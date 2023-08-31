using Microsoft.EntityFrameworkCore;
using SendMeetingsDemo.Data.Repositories;
using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Services
{
    public class MeetingRep : IMeetingRep
    {
        private SendReceiptContext _context;

        public MeetingRep(SendReceiptContext context)
        {
            _context = context;
        }

        public void AddMeeting(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            _context.SaveChanges();
        }

        public void EditMeeting(Meeting meeting)
        {
            _context.Meetings.Update(meeting);
            _context.SaveChanges();
        }

        public bool ExistMeeting(int meetingId)
        {
            return _context.Meetings.Any(e => e.MeetingId == meetingId);
        }

        public List<Meeting> GetAllMeetings()
        {
            return _context.Meetings.Include(c => c.Customer).Include(b => b.BankAccount).OrderByDescending(r => r.MeetingId).ToList();
        }

        public List<Meeting> GetOldMeetings()
        {
           return _context.Meetings.Include(c=>c.Customer).Include(b=>b.BankAccount).Where(m=> !m.Status.Contains("در حال بررسی") ).OrderByDescending(r => r.MeetingId).ToList();
        }

        public List<Meeting> GetOldMeetingsForPages(int skip)
        {
            return GetOldMeetings().Skip(skip).Take(20).ToList();
        }

        public List<Meeting> GetNewMeetings()
        {
            return _context.Meetings.Include(c => c.Customer).Include(b => b.BankAccount).Where(m => m.Status.Contains("در حال بررسی")).OrderByDescending(r => r.MeetingId).ToList();
        }

        public List<Meeting> GetNewMeetingsForPages(int skip)
        {
            return GetNewMeetings().Skip(skip).Take(20).ToList();
        }

        public Meeting GetMeetingById(int meetingId)
        {
            return _context.Meetings.Include(c => c.Customer).Include(b => b.BankAccount).SingleOrDefault(r=>r.MeetingId == meetingId);
        }
        
        public void RemoveMeeting(Meeting meeting)
        {
            _context.Meetings.Remove(meeting);
            _context.SaveChanges();
        }

        public void RemoveMeeting(int meetingId)
        {
            Meeting meeting = GetMeetingById(meetingId);
            RemoveMeeting(meeting);
        }

        public List<ScoreReqVM> GetScoreReqsForCustomer(string mobile)
        {
           var reqs= _context.Meetings.OrderByDescending(m=>m.MeetingId).Where(m=> m.ScorerMobileNumber == mobile && m.Status .Contains( "در حال بررسی") && m.PassedStatus == "0").Select(x=> new ScoreReqVM()
           {
               ReqAccountNumber = x.AccountNumber,
               ReqName = x.Customer.FullName,
               ReqScore =x.ReqScore,
               ScorerAccountNumber = x.ScorerAccountNumber,
               MeetId = x.MeetingId
           }).ToList();
            int index = 1;
            foreach (var item in reqs)
            {
                item.ReqIndex = index;
                ++ index;   
            }
            return reqs;
        }

        public List<BlockReqVM> GetBlockReqsForCustomer(string mobile)
        {
            var reqs = _context.Meetings.OrderByDescending(m => m.MeetingId).Where(m => m.BlockScorerMobileNumber == mobile && m.Status.Contains("در حال بررسی") && m.BlockPassedStatus == "0").Select(x => new BlockReqVM()
            {
                ReqAccountNumber = x.AccountNumber,
                ReqName = x.Customer.FullName,
                BlockReqScore = x.BlockReqScore,
                ScorerAccountNumber = x.BlockScorerAccountNumber,
                MeetId = x.MeetingId
            }).ToList();
            int index = 1;
            foreach (var item in reqs)
            {
                item.ReqIndex = index;
                ++index;
            }
            return reqs;
        }
    }
}
