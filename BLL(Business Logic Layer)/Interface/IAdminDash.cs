using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IAdminDash
    {
        public List<adminDash> adminData();
        public List<adminDash> adminDataViewCase(int reqId);
        public viewNotes adminDataViewNote(int reqId);
        public void adminDataViewNote(adminDashData obj);

        public CloseCase closeCaseNote(int reqId);
        public void closeCaseNote(adminDashData obj);

        public List<casetageNote> casetag();

        public AssignCase adminDataAssignCase(int req);
        public void adminDataAssignCase(adminDashData assignObj);
        
        public AssignCase adminDataAssignCaseDocList(int regionId);

        public blockCaseModel blockcase(int req);

        public void blockcase(adminDashData obj, string sessionEmail);

        public List<viewUploads> viewUploadMain(int reqId);

        public void viewUploadMain(adminDashData obj);
        public void DeleteFile(bool data,int reqFileId);
        public void sendMail(string emailMain, string[] data);

        public activeOrder viewOrder(int reqId);

        public activeOrder businessName(int profession_id);
        public activeOrder businessDetail(int businessDetail);

        public void viewOrder(adminDashData adminDashData);

        public transferRequest transferReq(int req);
        public void transferReq(adminDashData data,string sessionEmail);

        public blockCaseModel clearCase(int reqId);
        public void clearCase(adminDashData block,string sessionEmail);

        public sendAgreement sendAgree(int reqId);
        public void sendAgree(adminDashData dataMain);

        public closeCaseMain closeCaseMain(int reqId);

        public void closeCaseSaveMain(adminDashData obj);
        public void closeCaseCloseBtn(int reqId, string sessionEmail);
        public myProfile myProfile(string sessionEmail);

        public bool myProfileReset(myProfile obj, string sessionEmail);

        public myProfile myProfileAdminInfo(myProfile obj, string sessionEmail);
        public bool myProfileAdminBillingInfo(myProfile obj, string sessionEmail);

        public concludeEncounter concludeEncounter(int data);
        public concludeEncounter concludeEncounter(concludeEncounter obj);
        public sendLink sendLink(adminDashData data);
        public createRequest createRequest(createRequest data,string sessionEmail);

        public createRequest verifyState(string state);
    }
}
