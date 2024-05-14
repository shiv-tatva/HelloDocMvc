using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Services
{
    public class ChatHub:Hub
    {

        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Chat> _chatRepo;

        public ChatHub(ApplicationDbContext context,IGenericRepository<Chat> chatRepo)
        {
            _context = context;
            _chatRepo = chatRepo;
        }

        public async Task SendMessage(string message,string RequestID,string ProviderID,string AdminID,string RoleID,string GroupFlagID)
        {
            //if (RoleID != "1" && AdminID != "0")
            //{
            //    var adminData = _context.Admins.ToList();

            //    foreach (var item in adminData)
            //    {
            //        Chat chat = new Chat();
            //        chat.Message = message;
            //        chat.SentBy = Convert.ToInt32(RoleID);
            //        chat.AdminId = item.Adminid;
            //        chat.RequestId = Convert.ToInt32(RequestID);
            //        chat.PhyscainId = Convert.ToInt32(ProviderID);
            //        chat.SentDate = DateTime.Now;
            //        _chatRepo.Add(chat);
            //    }
            //}
            //else
            //{
                Chat chat = new Chat();
                chat.Message = message;
                chat.SentBy = Convert.ToInt32(RoleID);
                chat.AdminId = Convert.ToInt32(AdminID);
                chat.RequestId = Convert.ToInt32(RequestID);
                chat.PhyscainId = Convert.ToInt32(ProviderID);
                chat.SentDate = DateTime.Now;
                chat.ChatType = Convert.ToInt32(GroupFlagID);
                _chatRepo.Add(chat);
            //}

            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
