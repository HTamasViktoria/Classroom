using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;

namespace Classroom.Service
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(ILogger<NotificationService> logger, INotificationRepository notificationRepository)
        {
            _logger = logger;
            _notificationRepository = notificationRepository;
        }

        public void PostToDb(NotificationRequest request)
        {
         
            if (request.Date == default)
            {
                _logger.LogError("Hiba: A 'Date' mező kötelező.");
                throw new ArgumentException("A 'Date' mező kötelező.");
            }

       
            if (request.StudentIds == null || request.StudentIds.Count == 0)
            {
                _logger.LogError("Hiba: A 'Students' mező kötelező.");
                throw new ArgumentException("A 'Students' mező kötelező.");
            }

         
            if (string.IsNullOrEmpty(request.Description))
            {
                _logger.LogError("Hiba: A 'Description' mező kötelező.");
                throw new ArgumentException("A 'Description' mező kötelező.");
            }

           
            if ((request.Type == "Exam" || request.Type == "Homework" || request.Type == "MissingEquipment") && request.Subject == default)
            {
                _logger.LogError("Hiba: A 'Subject' mező kötelező az 'Exam', 'Homework' és 'MissingEquipment' típusú értesítéseknél.");
                throw new ArgumentException("A 'Subject' mező kötelező az 'Exam', 'Homework' és 'MissingEquipment' típusú értesítéseknél.");
            }

      
            _notificationRepository.Add(request);

            _logger.LogInformation("Értesítés sikeresen elmentve az adatbázisba.");
        }
    }
}
