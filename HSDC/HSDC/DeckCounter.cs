using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using mshtml;

namespace HSDC
{
   public class Options
   {
      private string OPTIONS_FILE = "hsdc.cfg";
      private readonly Dictionary<string, string> opt;

      public Options()
      {
         opt = new Dictionary<string, string>();

         try
         {
            if (File.Exists( OPTIONS_FILE ))
            {
               XmlDocument xml = new XmlDocument();
               xml.Load( OPTIONS_FILE );
               foreach ( XmlNode node in xml.SelectNodes( "//options/option" ) )
               {
                  string name = node.Attributes[ "name" ].Value;
                  string value = node.Attributes[ "value" ].Value;
                  opt.Add( name, value );
               }
            }
         }
         catch
         {
            MessageBox.Show( "There has been a problem loading the configuration file!" );
         }
      }

      ~Options()
      {
         XmlDocument xml = new XmlDocument();
         XmlNode dec = xml.CreateXmlDeclaration( "1.0", "UTF-8", null );
         xml.AppendChild( dec );
         XmlNode root = xml.CreateElement( "options" );
         xml.AppendChild( root );
         foreach ( KeyValuePair<string, string> o in opt )
         {
            XmlNode option = xml.CreateElement( "option" );
            XmlAttribute name = xml.CreateAttribute( "name" );
            name.Value = o.Key;
            XmlAttribute value = xml.CreateAttribute( "value" );
            value.Value = o.Value;
            option.Attributes.Append( name );
            option.Attributes.Append( value );
            root.AppendChild( option );
         }
         xml.Save( OPTIONS_FILE );
      }

      public void Set( string option, string value )
      {
         if (!opt.ContainsKey( option ))
         {
            opt.Add( option, value );
         }
         else
         {
            opt[ option ] = value;
         }
      }

      public bool GetAsBool( string option )
      {
         if (!opt.ContainsKey( option )) return false;
         return Convert.ToBoolean( opt[ option ] );
      }

      public int GetAsInteger( string option )
      {
         if (!opt.ContainsKey( option )) return 0;
         return Convert.ToInt32( opt[ option ] );
      }

      public string GetAsString( string option )
      {
         if (!opt.ContainsKey( option )) return "";
         return opt[ option ];
      }
   }

   public class DeckCounter
   {
      private string VERSION = "16";
      public Options Options;
      public MemoryReader MemoryReader;
      public List<Card> AvailableCards;
      public FormTooltip tooltip = null;
      public FormMain FormMain = null;
      private readonly FlowLayoutPanel Panel = null;
      public ComboBox ComboBoxRemoveCard = null;
      public Label LabelNumCards = null;
      public Label LabelTopdeck = null;

      // Init functions
      public DeckCounter( FlowLayoutPanel p )
      {
         Init();
         this.Panel = p;
      }

      public void Init()
      {
         CheckForUpdate();

         Options = new Options();
         if (Options.GetAsString( "CardShowImage" ) == "") Options.Set( "CardShowImage", "true" );
         if (Options.GetAsString( "CardColorByType" ) == "") Options.Set( "CardColorByType", "true" );
         if (Options.GetAsString( "CardColoredName" ) == "") Options.Set( "CardColoredName", "true" );
         if (Options.GetAsString( "CardShowTD" ) == "") Options.Set( "CardShowTD", "true" );
         if (Options.GetAsString( "CardHeight" ) == "") Options.Set( "CardHeight", "25" );
         if (Options.GetAsString( "CardAutoSort" ) == "") Options.Set( "CardAutoSort", "true" );
         if (Options.GetAsString( "CardDarkenValue" ) == "") Options.Set( "CardDarkenValue", "224" );

         if (Options.GetAsString( "StayOnTop" ) == "") Options.Set( "StayOnTop", "false" );
         if (Options.GetAsString( "LastDeck" ) == "") Options.Set( "LastDeck", "" );
         if (Options.GetAsString( "Language" ) == "") Options.Set( "Language", "en" );
         if (Options.GetAsString( "ConfirmOnDeckReset" ) == "") Options.Set( "ConfirmOnDeckReset", "true" );
         if (Options.GetAsString( "SortFirst" ) == "") Options.Set( "SortFirst", "mana" );
         if (Options.GetAsString( "SortSecond" ) == "") Options.Set( "SortSecond", "name" );
         if (Options.GetAsString( "SortThird" ) == "") Options.Set( "SortThird", "" );
         if (Options.GetAsString( "DisplayTooltips" ) == "") Options.Set( "DisplayTooltips", "true" );


         LoadCards();

         MemoryReader = new MemoryReader( this );
      }

      public void SetCardHeight( int height )
      {
         Options.Set( "CardHeight", height.ToString() );
         this.Panel.SuspendLayout();
         foreach ( CardControl cc in this.Panel.Controls )
         {
            cc.SetHeight( height );
         }
         this.Panel.ResumeLayout();
      }

      public void SetStayOnTop( bool state )
      {
         Options.Set( "StayOnTop", state.ToString() );
         FormMain.TopMost = state;
      }

      public void SetLanguage( string lang )
      {
         Options.Set( "Language", lang );
         LoadCards();
      }

      public void ShowTooltip( Card card )
      {
         if (tooltip == null)
         {
            tooltip = new FormTooltip( this, card );
         }
         else
         {
            tooltip.SetCard( card );
         }
         tooltip.Show();
      }

      public void CheckForUpdate()
      {
         /*
            WebClient wc = new WebClient();
            string version = wc.DownloadString("http://www.weaz.de/hsdc/version");
            if (Convert.ToInt32(version) > Convert.ToInt32(this.VERSION))
            {
                if (MessageBox.Show("There is an update for HSDC (new version: " + version + ") available! Do you want to visit the website?", "Update notice", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("http://www.weaz.de/hsdc");
                }
            }
            */
      }

      public void LoadCards()
      {
         AvailableCards = new List<Card>();

         try
         {
            var language = Options.GetAsString( "Language" );
            if (!File.Exists( "data/cards_" + language + ".xml" ))
            {
               //WebClient wc = new WebClient();
               //wc.DownloadFile("http://www.weaz.de/hsdc/cards.php?lang=" + language, "data/cards_" + language + ".xml");
               MessageBox.Show( "Card description files not found!" );
            }
            XmlDocument xml = new XmlDocument();
            xml.Load( "data/cards_" + language + ".xml" );
            foreach ( XmlNode node in xml.SelectNodes( "//cards/card" ) )
            {
               var c = new Card
               {
                  Id = Convert.ToInt32( node.SelectSingleNode( "id" ).InnerText ),
                  Quality = Convert.ToInt32( node.SelectSingleNode( "quality" ).InnerText ),
                  Name = node.SelectSingleNode( "name" ).InnerText,
                  Cost = Convert.ToInt32( node.SelectSingleNode( "cost" ).InnerText )
               };
               switch (Convert.ToInt32( node.SelectSingleNode( "type" ).InnerText ))
               {
                  case 4:
                     c.Type = "minion";
                     break;
                  case 5:
                     c.Type = "spell";
                     break;
                  case 7:
                     c.Type = "weapon";
                     break;
               }
               c.Image = node.SelectSingleNode( "image" ).InnerText;
               AvailableCards.Add( c );
            }

            if (Panel != null)
            {
               foreach ( CardControl cc in Panel.Controls )
               {
                  cc.Card.Name = AvailableCards.Find( x => x.Id == cc.Card.Id ).Name;
                  cc.Invalidate();
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show( "There has been a problem while contacting the server for card data: " + ex.Message + "\n" +
                             ex.StackTrace );
         }
      }

      // Adding and removing cards (deck managing)
      public string[] ListCardsInDeck()
      {
         List<string> cards = new List<string>();
         foreach ( CardControl cc in this.Panel.Controls )
         {
            cards.Add( cc.Card.Name );
         }
         return cards.ToArray();
      }

      public AutoCompleteStringCollection ListAvailableCards()
      {
         var a = new AutoCompleteStringCollection();
         foreach ( Card c in AvailableCards )
         {
            a.Add( c.Name );
         }
         return a;
      }

      public CardControl FindCardInDeck( string name )
      {
         return Panel.Controls.Cast<CardControl>().FirstOrDefault( cc => String.Equals( cc.Card.Name, name, StringComparison.CurrentCultureIgnoreCase ) );
      }

      public Card FindCardById( int id )
      {
         return AvailableCards.Find( c => c.Id == id );
      }

      public Card FindCardByImage( string image )
      {
         return AvailableCards.Find( c => c.Image == image );
      }

      public void AddCard( int id )
      {
         if (CardCount() >= 30) return;
         Card c = this.AvailableCards.Find( x => x.Id == id );
         if (c != null) AddCard( c.Name );
      }

      public void AddCard( string name )
      {
         if (CardCount() >= 30) return;
         CardControl cc = FindCardInDeck( name );
         if (cc != null)
         {
            cc.Card.Count++;
         }
         else
         {
            Card c = this.AvailableCards.Find( x => x.Name.ToLower() == name.ToLower() );
            if (c == null) return;
            cc = new CardControl( this, (Card) c.Clone() );
            cc.SetHeight( Options.GetAsInteger( "CardHeight" ) );
            this.Panel.Controls.Add( cc );
            if (ComboBoxRemoveCard != null) ComboBoxRemoveCard.Items.Add( c.Name );
         }
         Refresh();
         RefreshAllCards();
      }

      public void RemoveCard( string name )
      {
         CardControl c = FindCardInDeck( name );
         if (c != null)
         {
            this.Panel.Controls.Remove( c );
            this.ComboBoxRemoveCard.Items.RemoveAt( this.ComboBoxRemoveCard.SelectedIndex );
         }
      }

      public void EmptyDeck()
      {
         this.Panel.Controls.Clear();
         if (this.ComboBoxRemoveCard != null) this.ComboBoxRemoveCard.Items.Clear();
      }

      // Sorting
      public void Sort()
      {
         Sort( Options.GetAsString( "SortThird" ) );
         Sort( Options.GetAsString( "SortSecond" ) );
         Sort( Options.GetAsString( "SortFirst" ) );

         if (Options.GetAsBool( "CardAutoSort" ))
         {
            List<CardControl> list0 = new List<CardControl>();
            List<CardControl> listRest = new List<CardControl>();
            foreach ( CardControl c in this.Panel.Controls )
            {
               if (c.Card.Count == 0)
               {
                  list0.Add( c );
               }
               else
               {
                  listRest.Add( c );
               }
            }
            this.Panel.Controls.Clear();
            this.Panel.Controls.AddRange( list0.ToArray() );
            this.Panel.Controls.AddRange( listRest.ToArray() );
         }
      }

      public void Sort( string type )
      {
         this.Panel.SuspendLayout();

         List<CardControl> list = new List<CardControl>();
         foreach ( CardControl c in this.Panel.Controls )
         {
            list.Add( c );
         }

         this.Panel.Controls.Clear();

         list = SortList( list, type );

         foreach ( CardControl c in list )
         {
            this.Panel.Controls.Add( c );
         }

         this.Panel.ResumeLayout();
      }

      public List<CardControl> SortList( List<CardControl> list, string type )
      {
         switch (type)
         {
            case "name [<]":
               return list.OrderBy( x => x.Card.Name ).ToList();
            case "name [>]":
               return list.OrderBy( x => x.Card.Name ).Reverse().ToList();
            case "mana [<]":
               return list.OrderBy( x => x.Card.Cost ).ToList();
            case "mana [>]":
               return list.OrderBy( x => x.Card.Cost ).Reverse().ToList();
            case "count [<]":
               return list.OrderBy( x => x.Card.Count ).ToList();
            case "count [>]":
               return list.OrderBy( x => x.Card.Count ).Reverse().ToList();
            case "type [<]":
               return list.OrderBy( x => x.Card.Type ).ToList();
            case "type [>]":
               return list.OrderBy( x => x.Card.Type ).Reverse().ToList();
            default:
               return list;
         }
      }

      // Updating card data display
      public int CardCount()
      {
         int count = 0;
         foreach ( CardControl cc in this.Panel.Controls )
         {
            count += cc.Card.Count;
         }
         return count;
      }

      public int CardCountUnique()
      {
         return this.Panel.Controls.Count;
      }

      public void Refresh()
      {
         // Refresh card count
         this.LabelNumCards.Text = this.CardCount().ToString() + " / 30";
      }

      public void RefreshTDChance( int cardCount )
      {
         float chance = ( this.CardCount() != 0 ) ? 100f*cardCount/this.CardCount() : 0f;
         //this.LabelTopdeck.Text = Math.Round(chance, 2).ToString() + "%";
         this.LabelTopdeck.Text = Math.Round( chance, 0 ).ToString() + " %";
      }

      public void RefreshAllCards()
      {
         foreach ( CardControl cc in Panel.Controls )
         {
            cc.Refresh();
         }
      }

      // Deck loading and saving
      public void ResetDeck()
      {
         if (Options.GetAsString( "LastDeck" ) == "")
         {
            MessageBox.Show( "Could not reset the deck! Have you already saved it before playing with it?" );
            return;
         }
         LoadDeck( Options.GetAsString( "LastDeck" ) );
      }

      public void SaveDeck()
      {
         SaveFileDialog sfd = new SaveFileDialog
         {
            InitialDirectory = Application.StartupPath,
            DefaultExt = ".hsd",
            Filter = "Hearthstone deck|*.hsd"
         };
         if (sfd.ShowDialog() == DialogResult.OK)
         {
            var xml = new XmlDocument();
            XmlNode dec = xml.CreateXmlDeclaration( "1.0", "UTF-8", null );
            xml.AppendChild( dec );
            XmlNode root = xml.CreateElement( "deck" );
            XmlAttribute version = xml.CreateAttribute( "version" );
            version.Value = VERSION;
            root.Attributes.Append( version );
            xml.AppendChild( root );
            foreach ( CardControl cc in this.Panel.Controls )
            {
               XmlNode card = xml.CreateElement( "card" );
               XmlAttribute id = xml.CreateAttribute( "id" );
               id.Value = cc.Card.Id.ToString();
               XmlAttribute count = xml.CreateAttribute( "count" );
               count.Value = cc.Card.Count.ToString();
               card.Attributes.Append( id );
               card.Attributes.Append( count );
               root.AppendChild( card );
            }
            xml.Save( sfd.FileName );
            MessageBox.Show( "Decklist saved successfully!" );
         }
         Options.Set( "LastDeck", sfd.FileName );
      }

      public void LoadDeck( string file )
      {
         if (!File.Exists( file ))
         {
            MessageBox.Show( "Load Deck: File not found!" );
            return;
         }

         Panel.SuspendLayout();
         EmptyDeck();

         var xml = new XmlDocument();

         try
         {
            xml.Load( file );
            foreach ( XmlNode node in xml.SelectNodes( "//deck/card" ) )
            {
               var id = Convert.ToInt32( node.Attributes[ "id" ].Value );
               var count = Convert.ToInt32( node.Attributes[ "count" ].Value );
               for ( var i = 0; i < count; i++ )
               {
                  AddCard( id );
               }
            }
         }
         catch
         {
            MessageBox.Show(
               "There has been a problem while loading the deck file. Has it been created with an older version?" );
            return;
         }

         this.Panel.ResumeLayout();
         Sort();
         Options.Set( "LastDeck", file );
         FormMain.Text = "Hearthstone Deck Counter - " + file.Substring( file.LastIndexOf( "\\" ) + 1 );
      }

      public void LoadDeck()
      {
         OpenFileDialog ofd = new OpenFileDialog();
         ofd.InitialDirectory = Application.StartupPath;
         ofd.Filter = "Hearthstone deck|*.hsd";
         if (ofd.ShowDialog() == DialogResult.OK)
         {
            LoadDeck( ofd.FileName );
         }
      }

      // Deck scraping from websites
      public void LoadDeckFromWebsite( string url )
      {
         Form f = new Form();
         f.Size = new Size( 300, 50 );
         f.StartPosition = FormStartPosition.CenterScreen;
         f.FormBorderStyle = FormBorderStyle.FixedToolWindow;

         Label l = new Label();
         l.Text = "Scraping deck...please wait until this window disappears!";
         l.AutoSize = true;
         f.Controls.Add( l );

         f.BringToFront();
         f.Show();

         List<string> cards = null;
         if (url.Contains( "hearthhead.com" ))
         {
            if (url.Contains( "de.hearthhead.com" )) SetLanguage( "de" );
            if (url.Contains( "fr.hearthhead.com" )) SetLanguage( "fr" );
            if (url.Contains( "www.hearthhead.com" )) SetLanguage( "en" );
            cards = ScrapeHearthheadDeck( url );
         }
         if (url.Contains( "hearthpwn.com" ))
         {
            SetLanguage( "en" );
            cards = ScrapeHearthpwnDeck( url );
         }
         if (cards == null || cards.Count == 0)
         {
            MessageBox.Show( "There has been a problem parsing the website. Did you select one of the supported sites?" );
            f.Dispose();
            return;
         }

         this.Panel.SuspendLayout();
         this.EmptyDeck();

         foreach ( string c in cards )
         {
            AddCard( c );
         }

         this.Panel.ResumeLayout();
         Sort();
         Options.Set( "LastDeck", "" );

         f.Dispose();
      }

      private string InjectScript( WebBrowser wb, string script, string funcname )
      {
         HtmlElement head = wb.Document.GetElementsByTagName( "head" )[ 0 ];
         HtmlElement scriptEl = wb.Document.CreateElement( "script" );
         IHTMLScriptElement element = (IHTMLScriptElement) scriptEl.DomElement;
         element.text = script;
         head.AppendChild( scriptEl );

         object ret = wb.Document.InvokeScript( funcname );
         if (ret != null)
         {
            return ret.ToString();
         }
         else
         {
            return "";
         }
      }

      public List<string> ScrapeHearthheadDeck( string url )
      {
         WebBrowser wb = new WebBrowser();
         wb.ScriptErrorsSuppressed = true;
         wb.Navigate( url );
         while (wb.ReadyState != WebBrowserReadyState.Complete)
         {
            Application.DoEvents();
         }
         ;

         List<string> cards = new List<string>();
         string scr =
            "function scrape() { var ret = ''; $.each($('a.collapsed-card span.base'), function (i, v) { ret += $(v).children('span.name').text() + '°' + $(v).children('span.count').text() + '|'; }); return ret; }";
         string ret = InjectScript( wb, scr, "scrape" ).TrimEnd( '|' );
         foreach ( string s in ret.Split( '|' ) )
         {
            if (s != "")
            {
               string[] data = s.Split( '°' );
               if (data[ 0 ] != "")
               {
                  for ( int i = 0; i < Convert.ToInt32( data[ 1 ] ); i++ )
                  {
                     cards.Add( data[ 0 ] );
                  }
               }
            }
         }
         return cards;
      }

      public List<string> ScrapeHearthpwnDeck( string url )
      {
         WebBrowser wb = new WebBrowser();
         wb.ScriptErrorsSuppressed = true;
         wb.Navigate( url );
         while (wb.ReadyState != WebBrowserReadyState.Complete)
         {
            Application.DoEvents();
         }
         ;

         List<string> cards = new List<string>();
         string scr =
            "function scrape() { var ret = ''; $.each($('aside div a[data-tooltip-href]'), function (i, v) { ret += $(v).parent().parent().text().replace(/(\r\n|\n|\r)/gm, '').replace(' × ', '°') + '|'; }); return ret; }";
         string ret = InjectScript( wb, scr, "scrape" ).TrimEnd( '|' );

         foreach ( string s in ret.Split( '|' ) )
         {
            if (s != "")
            {
               string[] data = s.Split( '°' );
               if (data[ 0 ] != "")
               {
                  for ( int i = 0; i < Convert.ToInt32( data[ 1 ] ); i++ )
                  {
                     cards.Add( data[ 0 ] );
                  }
               }
            }
         }
         return cards;
      }
   }
}