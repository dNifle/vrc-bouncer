namespace VrChatBouncerBot
{
    public class FetchResult
    {
        public FetchResult()
        { 
            Successful = false;
            NumberOfAddedUsers = 0;
            NumberOfFetchedUsers = 0;
        }

        public FetchResult(int numberOfAddedUsers, int numberOfFetchedUsers)
        {
            Successful = true;
            NumberOfAddedUsers = numberOfAddedUsers;
            NumberOfFetchedUsers = numberOfFetchedUsers;
        }

        public bool Successful { get; }

        public int NumberOfAddedUsers { get; }
        public int NumberOfFetchedUsers { get; }
    }
}