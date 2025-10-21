using Volo.Abp.Domain.Entities.Auditing;

namespace CosmeticClinicManagement.Domain.ClinicManagement
{
    public class TreatmentPlan(Guid doctorId, Guid patientId) : FullAuditedAggregateRoot<Guid>
    {
        public Guid DoctorId { get; private set; } = doctorId;
        public Guid PatientId { get; private set; } = patientId;
        public List<Session> Sessions { get; private set; } = [new Session(DateTime.Now, [], SessionStatus.Planned)];
        public TreatmentPlanStatus Status { get; private set; } = TreatmentPlanStatus.Ongoing;

        public Session CurrentActiveSession()
        {
            var session = Sessions.SingleOrDefault(s => s.Status == SessionStatus.InProgress);
            return session ?? throw new InvalidOperationException("No active session found.");
        }

        private bool HasActiveSession()
        {
            return Sessions.Any(s => s.Status == SessionStatus.InProgress);
        }

        public void AddSession(Session session)
        {
            if (Status == TreatmentPlanStatus.Closed)
            {
                throw new InvalidOperationException("Cannot add session to a closed treatment plan.");
            }

            if (Sessions.Any(s => s.Id == session.Id))
            {
                throw new InvalidOperationException("Session with the same ID already exists in the treatment plan.");
            }

            if (HasActiveSession() && session.SessionDate < CurrentActiveSession().SessionDate)
            {
                throw new InvalidOperationException("New session date cannot be earlier than the current active session date.");
            }

            if (session.SessionDate < DateTime.Now)
            {
                throw new InvalidOperationException("Session date cannot be in the past.");
            }

            Sessions.Add(session);
        }

        public void Close()
        {
            if (Status == TreatmentPlanStatus.Closed)
            {
                throw new InvalidOperationException("Treatment plan is already closed.");
            }

            Status = TreatmentPlanStatus.Closed;
        }
    }
}
