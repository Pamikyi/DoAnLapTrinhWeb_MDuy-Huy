using DoAnLapTrinhWebBanThucAnNhanh.Models;
using System.Collections.Generic;

namespace DoAnLapTrinhWebBanThucAnNhanh.ViewModels
{
    public class UserProfileVM
    {
        public UserHL User { get; set; }
        public List<CustomerOrder> Orders { get; set; } = new();
    }
}
