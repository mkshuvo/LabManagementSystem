using System;

namespace LabManagementSystem.Shared
{
    public class BookingViewModel
    {
        
        public string UserId { get; set; }
        public int EquipmentId { get; set; }
        public DateTime BookDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool HasReturned{ get; set; }
        public bool IsApproved{ get; set; }
    }
}