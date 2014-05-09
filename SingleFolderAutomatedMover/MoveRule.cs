namespace SingleFolderAutomatedMover
{
    public class MoveRule
    {

        public string PathFrom { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PathTo { get; set; }

        public bool RequiresDifferentCredentials { get; set; }

        public MoveRule()
        {
        }

        public MoveRule(string pathFrom, string username, string password, string pathTo, bool requiresDifferentCredentials)
        {
            PathFrom = pathFrom;
            Username = username;
            Password = password;
            PathTo = pathTo;
            RequiresDifferentCredentials = requiresDifferentCredentials;
        }

        public MoveRule(string pathFrom, string pathTo, bool requiresDifferentCredentials)
        {
            PathFrom = pathFrom;
            PathTo = pathTo;
            //should always be false.
            RequiresDifferentCredentials = requiresDifferentCredentials;
        }
    }
}
