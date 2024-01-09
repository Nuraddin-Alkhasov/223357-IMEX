namespace HMI.Views.MainRegion.Protocol
{
    class Error
    {

        public Error(string _time, string _text, string _comment, string _user)
        {
            Time = _time;
            Text = _text;
            Comment = _comment;
            User = _user;
        }

        public string Time { set; get; }
       
        public string Text { set; get; }

        public string Comment { set; get; }
        
        public string User { set; get; }
    }
}
