using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using TwitterAccess;

namespace TwitterReader
{
	public partial class MainViewModel : BaseModel
    {
        private const string _GroupSettingsFileName = "GroupSettings.json";
        // Note: seems always default to 200 total with tweets + retweets, even if 300 is specified.
        private const int _MaxTweetsToRetrieve = 200;
        private UserEntity _loginUser;
        private bool _groupSettingsFileExists;
        private TwitterHttpClient _twitterHttpClient;
        private List<GroupModel> _groupModelList;
        private UserModel _selectedUserModel;
        private Dictionary<string, GroupImageChoice> _groupNameImageChoiceDic;
        private GroupImageChoice _lastUsedGroupImageChoice;

        public MainViewModel()
        {
            _twitterHttpClient = new TwitterHttpClient(App.TwitterCreds);
            RefreshCommand = new DelegateCommand(ExecuteRefreshCommand);
            OpenUrlCommand = new DelegateCommand<string>(ExecuteOpenUrlCommand);
            RefreshUserTweetsRetweetsCommand = new DelegateCommand(ExecuteRefreshUserTweetsRetweetsCommand);
            _groupNameImageChoiceDic = new Dictionary<string, GroupImageChoice>();
            _lastUsedGroupImageChoice = GroupImageChoice.None;
        }

        public string WindowTitle { get; private set; }
        public List<GroupModel> GroupModelList
        {
            get { return _groupModelList; }
            private set { SetProperty(ref _groupModelList, value); }
        }
        public UserModel SelectedUserModel
        {
            get { return _selectedUserModel; }
            set
            {
                SetProperty(ref _selectedUserModel, value);
                OnPropertyChanged("SelectedUserModelIsNull");
            }
        }
        public bool SelectedUserModelIsNull => _selectedUserModel == null;
        public string MoveToGroupMessage =>
                _groupSettingsFileExists ? "Move To Group" : $"{_GroupSettingsFileName} does not exist";
        public ICommand RefreshCommand { get; }
        public ICommand OpenUrlCommand { get; }
        public ICommand RefreshUserTweetsRetweetsCommand { get; }
        public string RefreshUserTweetsRetweetsTime { get; set; }

        public void Initialize()
        {
            if (App.TwitterCreds == null)
            {
                WindowTitle = "Twitter Reader - Twitter login user credentials invalid";
                return;
            }

            _loginUser = _twitterHttpClient.GetAuthenticatedUser();
            if (_loginUser == null)
            {
                WindowTitle = "Twitter Reader - Login user not authenticated";
                return;
            }

            List<Group> groupList = LoadGroupsFromFile();
            var groupModelList =
                    groupList.Select(x =>
                                    new GroupModel(x.Name, GetGroupImagePath(x.Name))
                                    {
                                        GroupScreenNamelList = x.ScreenNameList
                                    })
                             .ToList();
            RefreshFriends(groupModelList);
            WindowTitle = $"Login: {_loginUser.Name} (@{_loginUser.ScreenName})";
        }

        public void ExecuteSelectCommand(UserModel userModel)
        {
            try
            {
                List<TweetEntity> tweetList =
                    _twitterHttpClient.GetUserTweetList(
						userModel.UserId, 
						_MaxTweetsToRetrieve, 
						includeRetweet: true);
                userModel.TweetRetweetModelList = tweetList.Select(GenerateTweetModelFrom).ToList();
                userModel.TweetModelListWrapper.TweetModelList =
                                        userModel.TweetRetweetModelList.Where(x => !x.IsRetweet).ToList();
                userModel.RetweetModelListWrapper.TweetModelList =
                                        userModel.TweetRetweetModelList.Where(x => x.IsRetweet).ToList();
                SelectedUserModel = userModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ExecuteSelectCommand => {ex.Message}");
            }
        }

        public void ExecuteMoveUserToGroup(string userScreenName, string targetGroupName)
        {
            // Find current group for screenName
            var groupModel = GroupModelList
                                        .FirstOrDefault(x =>
                                            x.UserModelList.Any(y =>
                                                y.ScreenName == userScreenName));
            if (groupModel != null)
            {
                // Remove user from user list and update GroupScreenNamelList
                var userModel = groupModel.UserModelList.First(x => x.ScreenName == userScreenName);
                groupModel.UserModelList.Remove(userModel);
                groupModel.GroupScreenNamelList.RemoveAll(x => x == userScreenName);

                // Add user to target group and update GroupScreenNamelList
                groupModel = GroupModelList.First(x => x.GroupName == targetGroupName);
                groupModel.UserModelList.Add(userModel);
                userModel.GroupModel = groupModel;
                groupModel.GroupScreenNamelList.Add(userScreenName);

                // Update AvailableMoveToGroupList and rebind group model list                 
                var groupModelList = GroupModelList.ToList();
                UpdateAvailableMoveToGroupList(userModel, groupModelList);

                // Save groups every time a move occurs
                SaveGroupsToFile(groupModelList);

                // Update UI
                GroupModelList = groupModelList;
            }
        }

        private void RefreshFriends(List<GroupModel> groupModelList)
        {
            // Note: A friend is a user that the current Twitter login account follows.
            var friends = _twitterHttpClient.GetFriends(_loginUser.Id);
            foreach (var friend in friends)
            {
                var userModel = new UserModel(friend);
                InsertIntoGroup(userModel, groupModelList);
            }

            // Friends may have been changed (unfollowed), so remove those.
            // (If a new friend is followed, the friend automatically belongs to Default group.)
            groupModelList.ForEach(x => x.GroupScreenNamelList.RemoveAll(
                                    y => !friends.Any(
                                      z => z.ScreenName == y)));

            SaveGroupsToFile(groupModelList);
            GroupModelList = groupModelList;
        }

        private void SaveGroupsToFile(List<GroupModel> groupModelList)
        {
            try
            {
                string groupSettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _GroupSettingsFileName);
                List<Group> groupList =
                                groupModelList.Select(x =>
                                    new Group
                                    {
                                        Name = x.GroupName,
                                        ScreenNameList = x.UserModelList.Select(y => y.ScreenName).ToList()
                                    }).ToList();

                JsonHelper.SaveAsJsonToFile(groupList, groupSettingsFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(SaveGroupsToFile)} => {ex.Message}");
            }
        }

        private TweetModel GenerateTweetModelFrom(TweetEntity tweet)
        {
            // Split text and embed url
            string fullText, embedUrl;
            int index = tweet.FullText.IndexOf("http");
            if (index == 0)
            {
                fullText = string.Empty;
                embedUrl = tweet.FullText.Split(' ', '\n')?[0];
            }
            else if (index > 0)
            {
                fullText = tweet.FullText.Substring(0, index);
                embedUrl = tweet.FullText.Substring(index).Split(' ', '\n')?[0];
            }
            else
            {
                fullText = tweet.FullText;
                embedUrl = string.Empty;
            }

            // Determine tweet date / time
            string tweetDateTime;
            TimeSpan span = DateTime.Now.Subtract(tweet.CreatedAt);
            if ((int)span.TotalHours == 0)
            {
                tweetDateTime = $"{(int)span.TotalMinutes}m";
            }
            else if (span.TotalHours < 24)
            {
                int hours = (int)(span.TotalMinutes > 0 ? span.TotalHours + 1 : span.TotalHours);
                tweetDateTime = $"{hours}h";
            }
            else
            {
                tweetDateTime = string.Format("{0:t}   ", tweet.CreatedAt) +
                                        string.Format("{0:MMM d}", tweet.CreatedAt);
            }

            return new TweetModel
            {
                TweetId = tweet.Id,
                TweetUrl = $"https://twitter.com/{tweet.CreatedBy.ScreenName}/status/{tweet.Id}",
                TweetFullText = fullText,
                IsRetweet = tweet.RetweetedTweet != null,
                TweetEmbedUrl = embedUrl,
                TweetImageUrl = tweet.Entities?.MediaList?[0].MediaUrl,
                TweetDateTime = tweetDateTime,
            };
        }

        private void ExecuteRefreshCommand()
        {
            if (_loginUser != null)
            {
                var groupModelList = GroupModelList.ToList();
                groupModelList.ForEach(x => x.UserModelList.Clear());
                RefreshFriends(groupModelList);
            }
        }

        private void ExecuteOpenUrlCommand(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void ExecuteRefreshUserTweetsRetweetsCommand()
        {
            ExecuteSelectCommand(SelectedUserModel);
            // Note: SelectedUserModel as parameter above won't cause rebinding.
            //       Only TweetModelListWrapper.TweetModelList and RetweetModelListWrapper.TweetModelList
            //       may have changed, hence manually rebinding.
            SelectedUserModel.TweetModelListWrapper.OnPropertyChanged("TweetModelList");
            SelectedUserModel.RetweetModelListWrapper.OnPropertyChanged("TweetModelList");

            RefreshUserTweetsRetweetsTime = string.Format("@{0:T}", DateTime.Now);
            OnPropertyChanged(nameof(RefreshUserTweetsRetweetsTime));
        }

        private string GetGroupImagePath(string groupName)
        {            
            // Determine GroupImageChoice for groupName.
            GroupImageChoice imageChoice;
            if (groupName == Group.DefaultGroup)
            {
                imageChoice = GroupImageChoice.Blue;
            }
            else if (_groupNameImageChoiceDic.ContainsKey(groupName))
            {
                imageChoice = _groupNameImageChoiceDic[groupName];
            }
            else
            {
                if (_lastUsedGroupImageChoice == GroupImageChoice.None ||
                   ((int)_lastUsedGroupImageChoice + 1) == (int)GroupImageChoice.Last)
                {
                    // First new group that is not DefaultGroup, or a new wrapped-around group
                    imageChoice = GroupImageChoice.Blue + 1;
                }
                else
                {
                    imageChoice = _lastUsedGroupImageChoice + 1;
                }

                _groupNameImageChoiceDic[groupName] = imageChoice;
                _lastUsedGroupImageChoice = imageChoice;
            }

            return $"pack://application:,,,/Images/{imageChoice}.png";
        }

        private List<Group> LoadGroupsFromFile()
        {
            List<Group> groupList = null;
            string groupSettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _GroupSettingsFileName);
            _groupSettingsFileExists = File.Exists(groupSettingsFilePath);
            if (_groupSettingsFileExists)
            {
                groupList = JsonHelper.DeserializeFromFile<List<Group>>(groupSettingsFilePath);
                // DefaultGroup is hard-code and its users may have changed, so remove it
                groupList?.RemoveAll(x => x.Name == Group.DefaultGroup);
            }
            groupList = groupList ?? new List<Group>();
            // Add DefaultGroup as the last one
            groupList.Add(new Group { Name = Group.DefaultGroup });
            return groupList;
        }

        private void InsertIntoGroup(UserModel userModel, List<GroupModel> groupModelList)
        {
            var groupModel = groupModelList.FirstOrDefault(x =>
                                                 x.GroupScreenNamelList.Any(y =>
                                                    y == userModel.ScreenName));
            if (groupModel == null)
            {
                groupModel = groupModelList.First(x => x.GroupName.Equals(Group.DefaultGroup));
            }

            userModel.GroupModel = groupModel;
            groupModel.UserModelList.Add(userModel);
            UpdateAvailableMoveToGroupList(userModel, groupModelList);
        }

        private void UpdateAvailableMoveToGroupList(UserModel userModel,
                                                        List<GroupModel> groupModelList)
        {
            userModel.AvailableMoveToGroupList =
                       groupModelList
                           .Where(x => x.GroupName != userModel.CurrentGroupName)
                           .Select(x =>
                                   new ContextMenuItem
                                   {
                                       GroupName = x.GroupName,
                                       GroupImagePath = x.NodeImagePath
                                   })
                           .ToList();
        }
    }
}
