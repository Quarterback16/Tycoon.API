using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcJqGrid;
using ProgramAssuranceTool.Controllers;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.ViewModels.Project;

namespace ProgramAssuranceTool.Tests.Controllers
{   
    [TestClass]
	public class ProjectControllerTests
    {
        private ProjectController _underTest;
        private Mock<IPatService> _patService;
//        private Mock<IUserService> _userService;
        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpRequestBase> _request;
        private Mock<HttpResponseBase> _response;
        private Mock<HttpSessionStateBase> _session;
        private Mock<IControllerDependencies> _commonDependencies;
        private Mock<IVirtualPathService> _virtualPathService;
        private readonly string _gridDataProjectPath = Guid.NewGuid().ToString();

        [TestInitialize]
        public void Setup()
        {
            _session = new Mock<HttpSessionStateBase>();
            _httpContext = new Mock<HttpContextBase>();
            _request = new Mock<HttpRequestBase>();
            _response = new Mock<HttpResponseBase>();

            _httpContext.Setup(x => x.Request).Returns(_request.Object);
            _httpContext.Setup(x => x.Response).Returns(_response.Object);
            _httpContext.Setup(x => x.Session).Returns(_session.Object);

            _patService = new Mock<IPatService>();
//            _userService = new Mock<IUserService>();
            _commonDependencies = new Mock<IControllerDependencies>();
            _virtualPathService = new Mock<IVirtualPathService>();
            _virtualPathService.Setup(x => x.ToAbsolute(It.Is<string>(p => p == "~/Project/GridDataProject")))
                               .Returns(_gridDataProjectPath);

            _commonDependencies.Setup(x => x.VirtualPathService).Returns(_virtualPathService.Object);

//            _commonDependencies.Setup(x => x.BaseUser).Returns(_userService.Object);

            _commonDependencies.Setup(x => x.PatService).Returns(_patService.Object);

            _underTest = new ProjectController(_commonDependencies.Object);
            _underTest.ControllerContext = new ControllerContext()
                {
                    HttpContext = _httpContext.Object
                };
        }

        [TestCleanup]
        public void TearDown()
        {

        }
        
        [TestMethod]
        public void given_controller_when_get_action_index_then_model_is_ProjectListViewModel()
        {
            var result = _underTest.Index() as ViewResult;

            Assert.IsInstanceOfType(result.Model, typeof(ProjectListViewModel));
            
        }

        [TestMethod]
        public void given_controller_when_get_action_index_then_model_latest_earliest_GetEarliestAndLatestUploadDates()
        {
            var earliest = DateTime.Now.AddDays(5);
            var latest = DateTime.Now.AddDays(10); 

            _patService.Setup(x => x.GetEarliestAndLatestUploadDates(out latest))
                .Returns(earliest).Verifiable();

            var model = (_underTest.Index() as ViewResult).Model as ProjectListViewModel;

            _patService.Verify();

            Assert.AreEqual(earliest.Date, model.UploadFrom.Value.Date);
				Assert.AreEqual(earliest.Hour, model.UploadFrom.Value.Hour);
				Assert.AreEqual(earliest.Minute, model.UploadFrom.Value.Minute);
				Assert.AreEqual(earliest.Second, model.UploadFrom.Value.Second);

				Assert.AreEqual(latest.Date, model.UploadTo.Value.Date);
				Assert.AreEqual(latest.Date, model.UploadTo.Value.Date);
				Assert.AreEqual(latest.Hour, model.UploadTo.Value.Hour);
				Assert.AreEqual(latest.Minute, model.UploadTo.Value.Minute);
				Assert.AreEqual(latest.Second, model.UploadTo.Value.Second);

        }

        [TestMethod]
        public void given_controller_when_get_action_index_then_model_earliest_from_session()
        {
            var earliest = DateTime.Now.AddDays(5);
            var latest = new DateTime(1,1,1);

            _session.Setup(x => x[CommonConstants.SessionUploadedFrom]).Returns(earliest);


            var model = (_underTest.Index() as ViewResult).Model as ProjectListViewModel;

            _patService.Verify(x => x.GetEarliestAndLatestUploadDates(out latest),Times.Never());


				Assert.AreEqual(earliest.Date, model.UploadFrom.Value.Date);
				Assert.AreEqual(earliest.Hour, model.UploadFrom.Value.Hour);
				Assert.AreEqual(earliest.Minute, model.UploadFrom.Value.Minute);
				Assert.AreEqual(earliest.Second, model.UploadFrom.Value.Second);

				Assert.AreEqual(latest.Date, model.UploadTo.Value.Date);
				Assert.AreEqual(latest.Date, model.UploadTo.Value.Date);
				Assert.AreEqual(latest.Hour, model.UploadTo.Value.Hour);
				Assert.AreEqual(latest.Minute, model.UploadTo.Value.Minute);
				Assert.AreEqual(latest.Second, model.UploadTo.Value.Second);

        }

        [TestMethod]
        public void given_controller_when_get_action_index_then_model_latest_from_session()
        {
            var earliest = new DateTime(1, 1, 1);
            var latest = DateTime.Now.AddDays(5);

            _session.Setup(x => x[CommonConstants.SessionUploadedTo]).Returns(latest);


            var model = (_underTest.Index() as ViewResult).Model as ProjectListViewModel;

            _patService.Verify(x => x.GetEarliestAndLatestUploadDates(out latest), Times.Never());


				Assert.AreEqual(earliest.Date, model.UploadFrom.Value.Date);
				Assert.AreEqual(earliest.Hour, model.UploadFrom.Value.Hour);
				Assert.AreEqual(earliest.Minute, model.UploadFrom.Value.Minute);
				Assert.AreEqual(earliest.Second, model.UploadFrom.Value.Second);

				Assert.AreEqual(latest.Date, model.UploadTo.Value.Date);
				Assert.AreEqual(latest.Date, model.UploadTo.Value.Date);
				Assert.AreEqual(latest.Hour, model.UploadTo.Value.Hour);
				Assert.AreEqual(latest.Minute, model.UploadTo.Value.Minute);
				Assert.AreEqual(latest.Second, model.UploadTo.Value.Second);

        }

        [TestMethod]
        public void given_controller_when_get_action_index_then_ViewData_grid_is_Grid()
        {
            _underTest.Index();

            var grid = _underTest.ViewData["grid"];
            Assert.IsInstanceOfType(grid, typeof(Grid));

        }

        [TestMethod]
        public void given_controller_when_get_action_index_then_reset_the_active_project()
        {
            _underTest.Index();

            _session.VerifySet(x => x[CommonConstants.SessionProjectId] = 0,Times.Once(), "Reset did not happen");
        }

        [TestMethod]
        public void given_controller_when_get_action_index_then_grid_project_save_in_session()
        {
            _underTest.Index();

            var grid = _underTest.ViewData["grid"];
            _session.VerifySet(x => x[CommonConstants.SessionProjectGrid] = grid.ToString(), Times.Once());
        }

        [TestMethod]
        public void given_controller_when_post_action_index_model_valid_then_redirect_to_get_action_index()
        {
            var vm = new ProjectListViewModel
                {

                };

            var result = _underTest.Index(vm) as RedirectToRouteResult;

            _session.VerifySet(x => x[CommonConstants.SessionUploadedFrom] = AppHelper.ShortDate(vm.UploadFrom), Times.Once());
            _session.VerifySet(x => x[CommonConstants.SessionUploadedTo] = AppHelper.ShortDate(vm.UploadTo), Times.Once());

            Assert.AreEqual("Project",result.RouteValues["Controller"]);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod]
        public void given_controller_when_post_action_index_from_more_to_then_return_view()
        {
            var vm = new ProjectListViewModel
            {
                UploadTo = DateTime.Now.AddDays(-11),
                UploadFrom = DateTime.Now.AddDays(-10),
            };

            var grid = Guid.NewGuid().ToString();
            _session.Setup(x => x[CommonConstants.SessionProjectGrid]).Returns(grid);
            var result = _underTest.Index(vm) as ViewResult;

            _session.VerifySet(x => x[CommonConstants.SessionUploadedFrom] = AppHelper.ShortDate(vm.UploadFrom), Times.Never());
            _session.VerifySet(x => x[CommonConstants.SessionUploadedTo] = AppHelper.ShortDate(vm.UploadTo), Times.Never());


            Assert.AreEqual("Upload To date cannot be before the Upload From date", _underTest.ModelState["UploadTo"].Errors[0].ErrorMessage);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.AreEqual(vm, result.Model);
            Assert.AreEqual(grid, _underTest.ViewData["grid"]);

        }

        [TestMethod]
        public void given_controller_when_post_action_index_from_and_to_future_then_return_view()
        {
            var vm = new ProjectListViewModel
            {
                UploadTo = DateTime.Now.AddDays(1).Date,
                UploadFrom = DateTime.Now.AddDays(1).Date,
            };

            var grid = Guid.NewGuid().ToString();
            _session.Setup(x => x[CommonConstants.SessionProjectGrid]).Returns(grid);
            var result = _underTest.Index(vm) as ViewResult;

            _session.VerifySet(x => x[CommonConstants.SessionUploadedFrom] = AppHelper.ShortDate(vm.UploadFrom), Times.Never());
            _session.VerifySet(x => x[CommonConstants.SessionUploadedTo] = AppHelper.ShortDate(vm.UploadTo), Times.Never());


            Assert.AreEqual("Upload To date cannot be in the future", _underTest.ModelState["UploadTo"].Errors[0].ErrorMessage);
            Assert.AreEqual("Upload From date cannot be in the future", _underTest.ModelState["UploadFrom"].Errors[0].ErrorMessage);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.AreEqual(vm, result.Model);
            Assert.AreEqual(grid, _underTest.ViewData["grid"]);

        }


	}
}
