using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using sportprofiles.Models;
using Syncfusion.TreeView.Engine;

namespace sportprofiles.ViewModels
{
    public class PostsViewModel: INotifyPropertyChanged
	{
        public ICommand RefreshCommand { get; set; }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;

            set
            {
                isRefreshing = value;

                RaisedOnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private string conversationMessage = "";
        private ObservableCollection<Conversation> conversations;
        private string sendIcon;
        private string replyIcon;

        public ObservableCollection<Conversation> Conversations
        {
            get { return conversations; }
            set
            {
                conversations = value;
                RaisedOnPropertyChanged("Conversations");
            }
        }

        public string ReplyIcon
        {
            get { return replyIcon; }
            set
            {
                replyIcon = value;
                RaisedOnPropertyChanged("ReplyIcon");
            }
        }

        public string SendIcon
        {
            get { return sendIcon; }
            set
            {
                sendIcon = value;
                RaisedOnPropertyChanged("SendIcon");
            }
        }

        public string ConversationMessage
        {
            get { return conversationMessage; }
            set
            {
                conversationMessage = value;
                RaisedOnPropertyChanged("ConversationMessage");
            }
        }

        public string UserIcon
        {
            get;
            set;
        }

        public ICommand NewConversationCommand { get; private set; }

        public ICommand NewReplyCommand { get; private set; }

        public ICommand ReplyEditCommand { get; private set; }

        public ICommand ExpandActionCommand { get; private set; }

        public event EventHandler<ChatEventArgs> ConversationAdded;

        private void OnRefreshCommandExecuted() => Task.Run(() => DoRefreshPosts());

        async Task DoRefreshPosts()
        {
            IsRefreshing = true;
            Conversations.Clear();
            Conversations = new ObservableCollection<Conversation>();
            this.Conversations = await this.GenerateConversations();
            IsRefreshing = false;
           
        }

        protected virtual void OnConversationAdded(ChatEventArgs e)
        {
            Task.Run(() => DoRefreshPosts());
            EventHandler<ChatEventArgs> handler = ConversationAdded;
            handler?.Invoke(this, e);
        }

        public event EventHandler<ReplyEditEventArgs> ReplyTapped;

        protected virtual void OnReplyTapped(ReplyEditEventArgs e)
        {
            EventHandler<ReplyEditEventArgs> handler = ReplyTapped;
            handler?.Invoke(this, e);
        }

        public event EventHandler<ChatEventArgs> ReplyAdded;

        protected virtual void OnReplyAdded(ChatEventArgs e)
        {
            EventHandler<ChatEventArgs> handler = ReplyAdded;
            handler?.Invoke(this, e);
        }

        sportprofiles.Services.Messages  _messageSvc = new sportprofiles.Services.Messages();
        public PostsViewModel()
		{
            IsRefreshing = true;
            RefreshCommand = new Command(OnRefreshCommandExecuted);
            NewConversationCommand = new Command(OnConversationAdding);
            Conversations = new ObservableCollection<Conversation>();
            Task.Run(() => Initialize());
        }

        async Task Initialize()
        {
            this.Conversations = await this.GenerateConversations();
            ReplyEditCommand = new Command(OnInitializeReply);
            ExpandActionCommand = new Command(OnExpandAction);
            NewReplyCommand = new Command(OnReplyConversation);
        }

        private void OnReplyConversation(object sender)
        {
            try
            {
                var treeViewNode = sender as TreeViewNode;
                var content = (IChatMessageInfo)treeViewNode!.Content!;
                Conversation conversation = null!;
                if (content is Conversation)
                {
                    conversation = (Conversation)content;
                }
                else if (content is Reply)
                {
                    conversation = (Conversation)treeViewNode!.ParentNode!.Content!;
                }
                if (conversation != null && !string.IsNullOrWhiteSpace(content.ReplyMessage))
                {
                    var replies = conversation.Replies;

                    string userName = Preferences.Get("UserName", "");
                    string userImage = Preferences.Get("UserImage", "");
                    string img = "https://www.sportprofiles.space/images/members/default.png";
                    if (userImage != null || userImage != "")
                    {
                        img = "https://www.sportprofiles.space/images/members/" + userImage;
                    }

                    replies.Insert(replies.Count, new Reply
                    {
                        Message = content.ReplyMessage,
                        Date = DateTime.Now,
                        Name = userName,
                        ProfileIcon = img

                    });

                    var msg = content.ReplyMessage;
                    var pid = conversation.PostID;

                    //add it to database
                    int memberID = 0;
                    if (Preferences.Get("UserID", "") != null)
                    {
                        memberID = Convert.ToInt32(Preferences.Get("UserID", ""));
                    }

                    //refresh or display
                    conversation.Replies = replies;
                    content.IsInEditMode = false;
                    if (content is Conversation)
                        conversation.IsNeedExpand = true;

                    _messageSvc.SavePosts(memberID, pid, msg);
                    DoRefreshPosts();

                    OnReplyAdded(new ChatEventArgs() { ChatMessageItem = content, Conversation = conversation });
                }
                content.ReplyMessage = null!;
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        await App.Current.MainPage.DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(" General Error...", "A general error occured while you were using the application. The error has been logged and recorded for a specialist to look at. Try again in a bit later.", "Ok");
                    }
                });
            }
        }

        private void OnExpandAction(object sender)
        {
            var node = sender as TreeViewNode;
            node!.IsExpanded = !node.IsExpanded;
        }

        private void ResetEditMode()
        {
            foreach (Conversation conversation in this.Conversations)
            {
                if (conversation.IsInEditMode)
                {
                    conversation.IsInEditMode = false;
                    conversation.ReplyMessage = null!;
                }
                foreach (Reply reply in conversation.Replies)
                {
                    if (reply.IsInEditMode)
                    {
                        reply.IsInEditMode = false;
                        reply.ReplyMessage = null!;
                        break;
                    }
                }
            }
        }

        private void OnInitializeReply(object sender)
        {
            var content = (sender as TreeViewNode)!.Content;
            this.ResetEditMode();
            if (content is Conversation)
            {
                var conversation = (Conversation)content;
                conversation.IsInEditMode = true;
                conversation.IsNeedExpand = false;
            }
            else if (content is Reply)
            {
                Reply reply = (Reply)content;
                reply.IsInEditMode = true;
            }
            OnReplyTapped(new ReplyEditEventArgs() { Content = content! });
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }

        #endregion

        private void OnConversationAdding()
        {
            try
            {
                PostsViewModel instance = this;
                if (!string.IsNullOrWhiteSpace(instance.ConversationMessage))
                {
                    string userName = Preferences.Get("UserName", "");
                    string userImage = Preferences.Get("UserImage", "");
                    string img = "https://www.sportprofiles.space/images/members/default.png";
                    if (userImage != null || userImage != "")
                    {
                        img = "https://www.sportprofiles.space/images/members/" + userImage;
                    }

                    Conversation conversation = new Conversation
                    {

                        Message = instance.ConversationMessage,
                        Date = DateTime.Now,
                        Name = userName,
                        ProfileIcon = img,
                        //TextColor = Color.FromHex("#f23518")
                    };

                    //add to database
                    int memberID = 0;
                    if (Preferences.Get("UserID", "") != null)
                    {
                        memberID = Convert.ToInt32(Preferences.Get("UserID", ""));
                    }
                    _messageSvc.SavePosts(memberID, 0, conversation.Message);
                    DoRefreshPosts();

                    //add to current UI instance
                    //  instance.Conversations.Add(conversation);
                    OnConversationAdded(new ChatEventArgs() { Conversation = conversation });
                }
                instance.ConversationMessage = null;
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        await App.Current.MainPage.DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(" General Error...", "A general error occured while you were using the application. The error has been logged and recorded for a specialist to look at. Try again in a bit later.", "Ok");
                    }
                });
            }
        }

        public async Task<ObservableCollection<Conversation>> GenerateConversations()
        {
            try
            {
                int memberID = 0;
                if (Preferences.Get("UserID", "") != null)
                    memberID = Convert.ToInt32(Preferences.Get("UserID", ""));

                this.IsRefreshing = true;
                List<RecentPostsModel> result = await _messageSvc.GetRecentPosts();

                var conversationList = new ObservableCollection<Conversation>();
                if (result != null)
                {
                    foreach (var r in result)
                    {
                        string img = "https://www.sportprofiles.space/images/members/default.png";
                        if (r.PicturePath != null || r.PicturePath != "")
                        {
                            img = "https://www.sportprofiles.space/images/members/" + r.PicturePath;
                        }

                        var conv = new Conversation() { Name = r.MemberName!, Message = r.Description!, Date = Convert.ToDateTime(r.DatePosted), ProfileIcon = img, IsNeedExpand = false, PostID = Convert.ToInt32(r.PostID) };
                        if (conv != null)
                        {
                            //get children for post
                            List<RecentPostChildModel> cResult = await _messageSvc.GetChildPosts(Convert.ToInt32(r.PostID));
                            if (cResult != null)
                            {
                                if (cResult.Count != 0)
                                    conv.IsNeedExpand = true;

                                foreach (var c in cResult)
                                {
                                    string img2 = "https://www.sportprofiles.space/images/members/default.png";
                                    if (c.PicturePath != null || c.PicturePath != "")
                                    {
                                        img2 = "https://www.sportprofiles.space/images/members/" + c.PicturePath;
                                    }
                                    conv.Replies.Add(new Reply() { Name = c.MemberName!, Message = c.Description!, Date = Convert.ToDateTime(c.DateResponded), ProfileIcon = img2, PostID = Convert.ToInt32(r.PostID) });
                                }
                            }
                        }
                        conversationList.Add(conv!);
                    }
                }
                this.IsRefreshing = false;
                return conversationList;
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        await App.Current.MainPage.DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(" General Error...", "A general error occured while you were using the application. The error has been logged and recorded for a specialist to look at. Try again in a bit later.", "Ok");
                    }
                });
                return null;
            }
        }
    }

    public class ReplyEditEventArgs : EventArgs
    {
        public object? Content { get; set; }
    }

    public class ChatEventArgs : EventArgs
    {
        public object? ChatMessageItem { get; set; }
        public Conversation? Conversation { get; set; }
    }


}

