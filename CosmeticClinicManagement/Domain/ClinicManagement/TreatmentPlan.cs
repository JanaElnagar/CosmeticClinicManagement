using Volo.Abp.Domain.Entities.Auditing;

namespace CosmeticClinicManagement.Domain.ClinicManagement
{
    public class TreatmentPlan(Guid doctorId, Guid patientId) : FullAuditedAggregateRoot<Guid>
    {
        public Guid DoctorId { get; private set; } = doctorId;
        public Guid PatientId { get; private set; } = patientId;
        public List<Session> Sessions { get; private set; } = [new Session(DateTime.Now, [], SessionStatus.Planned)];
        public TreatmentPlanStatus Status { get; private set; } = TreatmentPlanStatus.Ongoing;

        private Session CurrentActiveSession()
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
            ThrowExceptionIfClosed();

            if (!IsValidSession(session))
            {
                throw new InvalidOperationException("Invalid session cannot be added to the treatment plan.");
            }

            Sessions.Add(session);
        }

        public void AddUsedMaterialToSession(Guid sessionId, UsedMaterial usedMaterial)
        {
            ThrowExceptionIfClosed();
            var session = GetSessionById(sessionId);
            session.AddUsedMaterial(usedMaterial);
        }

        public void RemoveSession(Guid sessionId)
        {
            ThrowExceptionIfClosed();
            var session = GetSessionById(sessionId);
            if (session.Status != SessionStatus.Planned)
            {
                throw new InvalidOperationException("Only planned sessions can be removed from the treatment plan.");
            }
            Sessions.Remove(session);
        }

        public void StartSession(Guid sessionId)
        {
            ThrowExceptionIfClosed();

            if (HasActiveSession())
            {
                throw new InvalidOperationException("There is already an active session in the treatment plan.");
            }

            var session = GetSessionById(sessionId);

            if (!IsNext(session))
            {
                throw new InvalidOperationException("Only the earliest planned session can be started.");
            }

            session.UpdateStatus(SessionStatus.InProgress);
        }

        public void CancelSession(Guid sessionId)
        {
            ThrowExceptionIfClosed();

            var session = GetSessionById(sessionId);
            session.UpdateStatus(SessionStatus.Cancelled);
        }

        public void MarkSessionAsCompleted(Guid sessionId)
        {
            ThrowExceptionIfClosed();

            var session = GetSessionById(sessionId);
            if (session.Status != SessionStatus.InProgress)
            {
                throw new InvalidOperationException("There is no active session!");
            }

            session.UpdateStatus(SessionStatus.Completed);
        }

        public void Close()
        {
            ThrowExceptionIfClosed();

            if (!Sessions.All(s => s.Status == SessionStatus.Cancelled || s.Status == SessionStatus.Completed))
            {
                throw new InvalidOperationException("You cannot close a plan with opened or planned sessions");
            }

            Status = TreatmentPlanStatus.Closed;
        }

        private bool IsValidSession(Session session)
        {
            if (Sessions.Any(s => s.Id == session.Id))
            {
                return false;
            }

            if (HasActiveSession() && session.SessionDate < CurrentActiveSession().SessionDate)
            {
                return false;
            }

            if (session.SessionDate < DateTime.Now.AddMinutes(-5))
            {
                return false;
            }

            return true;
        }

        private bool IsNext(Session session)
        {
            return session.SessionDate == Sessions.Where(s => s.Status == SessionStatus.Planned).Min(s => s.SessionDate);
        }

        private Session GetSessionById(Guid sessionId)
        {
            return Sessions.SingleOrDefault(s => s.Id == sessionId)
                ?? throw new InvalidOperationException("Session not found in the treatment plan.");
        }

        private void ThrowExceptionIfClosed()
        {
            if (Status == TreatmentPlanStatus.Closed)
                throw new InvalidOperationException("You cannot operate on a closed plan.");
        }
    }
}
