using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ParsElecom.HijriCalender
{
    public enum CalendarType
    {
        Gregorian,
        Persian
    };

    public enum FirstDayOfWeek
    {
        Saturday = 6,
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5
    };

    [ToolboxBitmap(typeof(DatePicker), "ParsElecom.HijriCalender.CalendarToolbox.bmp")]
    public class DatePicker : TextBox
    {
        #region Fields

        #endregion

        #region Properties

        [Bindable(false)]
        [Category("Appearance")]
        [ReadOnly(false)]
        [DefaultValue(CalendarType.Persian)]
        [Description("Direction")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public CalendarType CalendarType
        {
            get
            {
                object calendarType = ViewState["rhpCalendarType"];
                return (calendarType == null) ? CalendarType.Persian : (CalendarType)calendarType;
            }
            set { ViewState["rhpCalendarType"] = value; }
        }

        [Bindable(false)]
        [Category("Appearance")]
        [ReadOnly(false)]
        [DefaultValue(FirstDayOfWeek.Saturday)]
        [Description("Direction")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public FirstDayOfWeek FirstDayOfWeek
        {
            get
            {
                object firstDayOfWeek = ViewState["rhpFirstDayOfWeek"];
                return (firstDayOfWeek == null) ? FirstDayOfWeek.Saturday : (FirstDayOfWeek)firstDayOfWeek;
            }
            set { ViewState["rhpFirstDayOfWeek"] = value; }
        }

        [Bindable(true),
         Category("Appearance"),
         //Description("Text that describes the purpose of the TextBox."),
         Localizable(true)]
        public string LabelText
        {
            get
            {
                var s = (String)ViewState["rhpLabelText"];
                return s ?? String.Empty;
            }
            set { ViewState["rhpLabelText"] = value; }
        }

        [Bindable(true),
         Category("Appearance"),
         Description("If True then the calendar will display week numbers."),
         Localizable(true)]
        public bool ShowWeekNumbers
        {
            get
            {
                var showWeekNumbers = (bool?)ViewState["rhpShowWeekNumbers"];
                return showWeekNumbers ?? false;
            }
            set { ViewState["rhpShowWeekNumbers"] = value; }
        }

        [Bindable(true),
         Category("Appearance"),
         Description("If set to True then days belonging to months overlapping with the currently displayed month will also be displayed in the calendar (but in a \"faded-out\" color)."),
         Localizable(true)]
        public bool ShowOthers
        {
            get
            {
                var showOthers = (bool?)ViewState["rhpShowOthers"];
                return showOthers ?? false;
            }
            set { ViewState["rhpShowOthers"] = value; }
        }

        [Bindable(true),
         Category("Appearance"),
         Description("Whether user can edit Textbox or not."),
         Localizable(true)]
        public new bool ReadOnly
        {
            get
            {
                var readOnly = (bool?)ViewState["rhpReadOnly"];
                return readOnly ?? false;
            }
            set { ViewState["rhpReadOnly"] = value; }
        }

        [Bindable(true),
         Category("Appearance"),
         Description("function that gets called when a date is selected.  You don't have to supply this (the default is generally okay)."),
         Localizable(true)]
        public string OnSelect
        {
            get
            {
                var onSelect = (string)ViewState["rhpOnSelect"];
                return onSelect;
            }
            set { ViewState["rhpOnSelect"] = value; }
        }

        [Bindable(true),
         Category("Appearance"),
         Description("If you supply a function handler here, it will be called right after the target field is updated with a new date. You can use this to chain 2 calendars, for instance to setup a default date in the second just after a date was selected in the first."),
         Localizable(true)]
        public string OnUpdate
        {
            get
            {
                var onUpdate = (string)ViewState["rhpOnUpdate"];
                return onUpdate;
            }
            set { ViewState["rhpOnUpdate"] = value; }
        }

        [Bindable(true),
         Category("Appearance"),
         Description("This handler will be called when the calen-dar needs to close. You don't need to provide one, but if you do it's your responsibility to hide/destroy the calendar. You're on your own."),
         Localizable(true)]
        public string OnClose
        {
            get
            {
                var onClose = (string)ViewState["rhpOnClose"];
                return onClose;
            }
            set { ViewState["rhpOnClose"] = value; }
        }

        #endregion Properties

        #region Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Type typeOfThis = this.GetType();
            ClientScriptManager scriptManager = this.Page.ClientScript;
            scriptManager.RegisterClientScriptResource(typeOfThis, "ParsElecom.HijriCalender.Javascript.datepicker.js");

            // CSS           
            //var style = (HtmlGenericControl) this.Page.Header.FindControl("DatePickerStyle");
            //if (style == null) 
            //{
            HtmlLink csslink = new HtmlLink();
            csslink.Attributes.Add("href", Page.ClientScript.GetWebResourceUrl(typeOfThis, "ParsElecom.HijriCalender.Style.theme.css"));
            csslink.Attributes.Add("type", "text/css");
            csslink.Attributes.Add("rel", "stylesheet");
            //csslink.InnerText = "p { font-weight: bold; }"; 
            Page.Header.Controls.Add(csslink);
            //}
        }

        protected override bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            bool returnValue = base.LoadPostData(postDataKey, postCollection);

            return returnValue;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "border: none;padding: 0;");

            if (ReadOnly)
                writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");

            base.Render(writer);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.Write(@"<span style=""display;inline-block;"">{0}&nbsp;</span>", LabelText);
            writer.WriteFullBeginTag("div style=\"padding: 2px 4px 4px 4px;position: relative;display: inline-block;border:1px solid #CCCCCC\"");//border:1px solid #CCCCCC;
            base.RenderControl(writer);
            RenderPictureButton(writer);
            writer.WriteEndTag("div");

            //writer.Write("",);
        }

        private void RenderPictureButton(HtmlTextWriter writer)
        {
            string hlId = string.Format("{0}hl", this.UniqueID);

            writer.WriteBeginTag("a");
            writer.WriteAttribute(HtmlTextWriterAttribute.Id.ToString(), hlId);
            writer.WriteAttribute(HtmlTextWriterAttribute.Href.ToString(), "#");
            writer.Write(" />");
            writer.WriteBeginTag("img");
            writer.WriteAttribute(HtmlTextWriterAttribute.Style.ToString(), "vertical-align:bottom;border:0px");
            writer.WriteAttribute(HtmlTextWriterAttribute.Id.ToString(), string.Format("{0}imgbt", this.UniqueID));
            writer.WriteAttribute(HtmlTextWriterAttribute.Src.ToString(),
                                  Page.ClientScript.GetWebResourceUrl(this.GetType(), "ParsElecom.HijriCalender.CalendarToolbox.bmp"));
            writer.WriteAttribute(HtmlTextWriterAttribute.Alt.ToString(), "کلیک نمایید");
            //writer.WriteAttribute(HtmlTextWriterAttribute.Onclick.ToString(), string.Format("displayDatePicker('{0}');", this.ClientID));
            writer.WriteAttribute("onload",
                                  string.Format("CalendarSetup('{0}', '{1}', '{2}', {3}, {4}, {5}, {6}, {7}, {8});", this.ClientID,
                                                hlId,
                                                CalendarType == CalendarType.Persian ? "jalali" : "gregorian",
                                                ShowWeekNumbers.ToString().ToLower(),
                                                ShowOthers.ToString().ToLower(),
                                                (int)FirstDayOfWeek,
                                                string.IsNullOrWhiteSpace(OnSelect) ? "null" : OnSelect,
                                                string.IsNullOrWhiteSpace(OnClose) ? "null" : OnClose,
                                                string.IsNullOrWhiteSpace(OnUpdate) ? "null" : OnUpdate));
            //writer.WriteAttribute(HtmlTextWriterAttribute.Class.ToString(), "imgcls");
            writer.Write(" />");
            writer.WriteEndTag("a");
        }

        private static string GetPersianDateString(DateTime? dt)
        {
            if (dt == null) return string.Empty;

            var calendar = new PersianCalendar();
            int year = calendar.GetYear(dt.Value);
            int month = calendar.GetMonth(dt.Value);
            int day = calendar.GetDayOfMonth(dt.Value);

            string text = string.Format("{0}/{1}/{2}", year, month.ToString().PadLeft(2, '0'), day.ToString().PadLeft(2, '0'));

            return text;
        }

        private static DateTime? GetDateTime(string persianDateString)
        {
            var calendar = new PersianCalendar();
            string[] dateArray = persianDateString.Split("/".ToCharArray());
            try
            {
                int year = int.Parse(dateArray[0]);
                int month = int.Parse(dateArray[1]);
                int day = int.Parse(dateArray[2]);
                return calendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch
            { /* ! it means value is null ! */
                return null;
            }
        }

        #endregion

        #region Properties

        [Bindable(true)]
        [Category("Appearance")]
        public DateTime? Date
        {
            get
            {
                var date = (DateTime?)ViewState["rhpDate"];
                return date;
            }
            set
            {
                ViewState["rhpDate"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        public string DatePersian
        {
            get
            {
                return GetPersianDateString(Date);
            }
            set
            {
                Date = GetDateTime(value);
            }
        }

        public override string Text
        {
            get
            {
                DateTime? date = Date;

                return (CalendarType == CalendarType.Gregorian)
                           ? date.HasValue ? date.Value.Date.ToString("yyyy/MM/dd") : string.Empty
                           : DatePersian;
            }
            set
            {
                DateTime? date = (CalendarType == CalendarType.Gregorian) ? DateTime.Parse(value) : GetDateTime(value);
                Date = date;
                base.Text = date.HasValue ? value : string.Empty;
            }
        }

        private new TextBoxMode TextMode
        {
            get
            {
                return base.TextMode;
            }
            set
            {
                base.TextMode = value;
            }
        }

        #endregion
    }
}