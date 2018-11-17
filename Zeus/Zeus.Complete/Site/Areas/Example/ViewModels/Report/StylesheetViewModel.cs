using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Report
{
    [DisplayName("Stylesheet")]
    [Group("CSS", Order = 1)]
    public class StylesheetViewModel
    {

        [Display(GroupName = "CSS", Order = 1)]
        public ContentViewModel StylesheetContent
        {
            get
            {
                return new ContentViewModel()
                .AddTitle("Stylesheet used")
                .AddStrongText("Familiarise yourself with various classes used in this stylesheet. You’ll want to supply these class names into the parameters of helper methods when rendering HTML controls. ")
                .AddPreformatted(@"
<style type='text/css'>
            
            /*  Common styles that apply for both Media Screen and Print. */
            
            @if (!string.IsNullOrEmpty(styleSection))
            {
                @Include(styleSection)
            }
            
             /*  Common styles that apply for both Media Screen and Print. */
            
            body
            { 
                font-family: Tahoma, Geneva, sans-serif;
            }
            
            
                        
            .newspaper
            {
                -webkit-column-count:3; /* Chrome, Safari, Opera */
                -moz-column-count:3;  /*Firefox */
                column-count:3;
            }
            
            
            /* This is used to capture data */
            label
            {
                display: block;
                float: left;
                clear: left;
                width: 25%;
                margin: 0.375em 0 0 0;
            }
            
          
        
		
		    .Signature
            {
                border-width: 0.000em;
                border-bottom: dotted thin #666;
                height: 1.563em;
                width: 70%;
                display:block;
                float:left;
                clear:left;

            }
		
		
		    .DateDDMMYY            
            {
                border-width: 0.000em;
                border-bottom: dotted thin #666;
                height: 1.563em;
                width: 8%;
            }
                                                
            .Date            
            {
                border-width: 0.000em;
                border-bottom: dotted thin #666;
                height: 1.563em;
                width: 20%;
                float:left;

            }

		
		
            table
            {
                width: 100%;
                border-collapse: collapse;
            }
            
            table th, td
            {
                width: auto;
                vertical-align: text-top;
                text-align: left;
                padding: 0.625em 0.625em 0.625em 0.625em;
            }
            
            
            /* Caption can be hidden or displayed */
        
            caption
            {
                text-align: left;
				padding-top: 0.625em;
            }
        
            caption.hide
            {
                display: none;
            }
            
            
            /* Ensures that paragraph text moves to the next line and provides enough spacing for the paragraph */
        
            p
            {
                clear: left;
                margin-bottom: 1.250em;
            }
            
            
            /* The following section allows for the styling of letter header */
        
            .letterheadcentre
            {
                text-align: center;
            }
        
            h1
            {
                text-align: center;
                font-size: 1.63em ;
                clear:left;

            }
            
            h2
            {
                font-size: 20px;
            }
            
            h3
            {
                font-size: 16px;
            }

            
            
            /* Generic floats */
        
            .floatright
            {
                float: right;
            }
            .floatleft
            {
                float: left;
            }
            
            .floatcentre
            {
                margin: auto;
            }

            
            .borderpara
            {
                border:thin solid #666; 
                padding: 0.63em;
            }

            .borderedcontainer
            {
                float:left; 
                width:100%; 
                clear:left; 
                border:solid thin #666;
                margin-bottom: 1.250em;

            }

            .bordcol1
            {
                float:left;
                width:73%; 
                border-right:solid thin #666; 
                padding: 0.63em;
                display:block; 
            }

            .bordcol2
            {
                float:left;
                width: auto; 
                padding: 0.63em; 
                text-align: center;
                display:block; 
            }
          
            
		
            /* This is used to layout label and data*/
        
            .fullrow
            {
                width: 100%;
                clear: left;
                display:block; 
            }
            .bindlabeltodata
            {
                width: 100%;
                display:block; 
            }
            .bindlabeltodatatwo
            {
                width: 50%;
                float: left;
                display:block; 
            }        
        
            
			.Label
            {
                
                float: left;
                font-weight: 800;
                padding-top: 0.375em;
                padding-bottom: 0.375em;
				display:block; 
			}
			
			.label50
			{
			 width: 50%;
            }
			
			.label25
			{
			 width: 25%;
			 
            }
			
			.data
            {
                display: block;
                float: left;
                padding-top: 0.375em ;
                padding-bottom: 0.375em;
				width:auto ;
            }
        
            ul.noindentorbullet
		    {
			    list-style:none; 
			    padding: 0;
			    margin: 0;
		    }
			  
        /* &&& */        
        @@media screen
        { 
            
               
           
       
            @if (! string.IsNullOrEmpty(screenStyleSection) )
            {@Include(screenStyleSection)}  
            
            
            /*  */
            body
            {
                background-color: #F9F9F9;
            }
        
            /* the outer div centres the form*/
            .outer
            {
                margin: auto;
                padding: 0.625em;
                width: 52.625em;
                background-color: #ffffff;
            }  
            
        
           /* Used to layout labels and data */  
           
            table thead th
            {
                background: #CACACA;
                color: #333;
                padding: 0.625em;
            }
            table tr:nth-child(odd)
            {
               background-color: #eee;
            }
            table tr:nth-child(even)
            {
                background-color: #fff;
            }
        
        
            /* Caption can be hidden or displayed */
        
            caption
            {
                font-size: 1.250em;
            }
        
        
            .data
            {
                width: 50%;
            }  
            
            
            
        }
        
        @@media print   /*###*/
        /*--*/{
            
            
                       
            
            @if ( ! string.IsNullOrEmpty(printStyleSection) )
            {@Include(printStyleSection)}
            
            
            
            
            body
            {
                background-color: #FFF;
            }
                      
            .page-break 
            {
                 display: block; 
                 page-break-before: always;
            }
            
            .defaultFooter
            {
                position: fixed;
                bottom: 0;
            }
        
            .defaultHeader
            {
                position: fixed;
                top: 0;
            }
        
            #pageNumber
            {
                counter-increment: page;
            }

            #pageNumber:after
            {
                content: counter(page);
            }
        
        
            /* Used to layout labels and data */  

            table th, td
            {
                border-style:solid;
                border-width: 0 0 0.063em 0;
                border-color: #999 ;
            }
            table th 
            {
                border-style:solid;
                border-width: 0.063em 0 0.063em 0;
                border-color: #999;
            }
			
			table tr:nth-child(odd)
            {
               background-color:  #fff;
            }
            table tr:nth-child(even)
            {
                background-color: #fff;
            }
			          
        /*--*/}
        </style>
                        ")    
                    ;
            }
        }
    }
}