using SendReceiptsDemo.Models;

namespace SendMeetingsDemo.Data.Repositories
{
    public interface IMeetingRep
    {
        public List<Meeting> GetAllMeetings();
        public List<Meeting> GetNewMeetings(); 
        public List<Meeting> GetNewMeetingsForPages(int skip);
        public List<Meeting> GetOldMeetings();
        public List<Meeting> GetOldMeetingsForPages(int skip);
        public Meeting GetMeetingById(int meetingId);
        public void AddMeeting(Meeting meeting);
        public void EditMeeting(Meeting meeting);
        public void RemoveMeeting(Meeting meeting);
        public void RemoveMeeting(int meetingId);
        public bool ExistMeeting(int meetingId);
        public List<ScoreReqVM> GetScoreReqsForCustomer(string mobile);
        public List<BlockReqVM> GetBlockReqsForCustomer(string mobile);
    }
}
