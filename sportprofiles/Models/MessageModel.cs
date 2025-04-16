using System;
namespace sportprofiles.Models
{

    public class MessageInfoModel
    {
        public string? Attachement { get; set; }
        public string? Body { get; set; }
        public string? ContactName { get; set; }
        public string? ContactImage { get; set; }
        public string? SenderImage { get; set; }
        public string? ContactID { get; set; }
        public string? FlagLevel { get; set; }
        public string? ImportanceLevel { get; set; }
        public string? MessageID { get; set; }
        public string? MessageState { get; set; }
        public string? SenderID { get; set; }
        public string? Subject { get; set; }
        public string? MsgDate { get; set; }
        public string? FromID { get; set; }
        public string? FirstName { get; set; }
        public string? FullBody { get; set; }
        public string? SenderTitle { get; set;}
    }

    public class MessageDetails
    {
        public string? MessageID { get; set; }
        public string? SenderID { get; set; }
        public string? SentDate { get; set; }
        public string? From { get; set; }
        public string? SenderPicture { get; set; }
        public string? Body { get; set; }
        public string? Subject { get; set; }
    }

}
