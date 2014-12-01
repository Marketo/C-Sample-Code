using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace Marketo_SOAP_API_Sample_Project
{
    public partial class Default : System.Web.UI.Page
    {

        // Added a reference to the Marketo Service to the ASP.NET project by right clicking on the project and selecting Add Service Reference
        //Endpoint can be found here - MLM->Admin->SOAP API->SOAP Endpoint (be sure to postfix with ?WSDL)
        
        // For example this Project points to the following Service Reference https://na-d.marketo.com/soap/mktows/2_0?WSDL

        // ALSO, you need to change the UserID and EncryptionID to match your account found here - MLM->Admin->SOAP API

        // You can also modify the endpoint address used in this example by changing it in the web.config
        // <client>
        //     <endpoint address="https://na-d.marketo.com/soap/mktows/2_0"
        //         binding="basicHttpBinding" bindingConfiguration="MktowsApiSoapBinding"
        //         contract="Marketo_WS_2_0.MktowsPort" name="MktowsApiSoapPort" />
        // </client>

        

        private Marketo_WS_2_0.MktowsPortClient client = new Marketo_WS_2_0.MktowsPortClient();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region User Clicked GetMultipleLeads by Date
        protected void btnGetMultipleLeadsDate_Click(object sender, EventArgs e)
        {
            Marketo_WS_2_0.ResultGetMultipleLeads resultReqGetMultipleLeads = new Marketo_WS_2_0.ResultGetMultipleLeads();

            string result = GetMultipleLeadsByDate(txtGetMultipleLeadsDate.Text, ref resultReqGetMultipleLeads);
            lblResultGMLDate.Text = result;
            lblResultGMLDateData.Text = "Found " + resultReqGetMultipleLeads.returnCount + " leads.";

        }
        #endregion

        #region User Clicked GetMultipleLeads by List
        protected void btnGetMultipleLeadsList_Click(object sender, EventArgs e)
        {
            Marketo_WS_2_0.ResultGetMultipleLeads resultReqGetMultipleLeads = new Marketo_WS_2_0.ResultGetMultipleLeads();

            string result = GetMultipleLeadsByList(txtGetMultipleLeadsList.Text, ref resultReqGetMultipleLeads);
            lblResultGMLList.Text = result;
            lblResultGMLListData.Text = "Found " + resultReqGetMultipleLeads.returnCount + " leads.";
        }
        #endregion

        #region User Clicked GetMultipleLeads by Lead List
        protected void btnGetMultipleLeadsLeadList_Click(object sender, EventArgs e)
        {
            Marketo_WS_2_0.ResultGetMultipleLeads resultReqGetMultipleLeads = new Marketo_WS_2_0.ResultGetMultipleLeads();

            string result = GetMultipleLeadsByListID(txtGetMultipleLeadsLeadsList.Text, ref resultReqGetMultipleLeads);
            lblResultGMLListID.Text = result;
            lblResultGMLListIDData.Text = "Found " + resultReqGetMultipleLeads.returnCount + " leads.";
        }
        #endregion

        #region User Clicked Request Campaign
        protected void btnRequestCampaign_Click(object sender, EventArgs e)
        {
            #region make the requestCampaign Call


            Marketo_WS_2_0.ResultRequestCampaign resultReqCampaign = new Marketo_WS_2_0.ResultRequestCampaign();

            string result = RequestCampaign(txtReqMarketoLead.Text, txtReqCampaignID.Text, ref resultReqCampaign);
            lblResultRequestCampaign.Text = result;

            #endregion


        }
        #endregion

        #region User Clicked Get Leads
        protected void btnGetLeads_Click(object sender, EventArgs e)
        {
            #region make the GetLeads Call
            string leadKey = txtLeadID.Text;

            Marketo_WS_2_0.ResultGetLead resultGetLead = new Marketo_WS_2_0.ResultGetLead();

            string result = GetLeads(leadKey, ref resultGetLead);
            #endregion

            #region Display the Lead info on the screen


            lblResult.Text = "";

            string allLeads = "";

            if (result.Length > 0)
            {
                allLeads = result;
            }
            else if (resultGetLead.count == 0)
            {
                allLeads = "No Lead(s) Found";
            }
            else
            {
                Marketo_WS_2_0.LeadRecord[] leadRecs = resultGetLead.leadRecordList;

                allLeads = "";

                foreach (Marketo_WS_2_0.LeadRecord leadRecord in leadRecs)
                {
                    allLeads = allLeads + @"<br/><b>" + leadRecord.Email + @"</b><br/>";
                    foreach (Marketo_WS_2_0.Attribute attrib in leadRecord.leadAttributeList)
                    {
                        allLeads = allLeads + attrib.attrName + " - " + attrib.attrValue + @"<br/>";
                    }
                }

            }



            lblResult.Text = allLeads;
            #endregion
        }
        #endregion


        #region User Clicked SyncLead

        protected void btnSyncLead_Click(object sender, EventArgs e)
        {
            #region make the syncLead Call
            string leadEmail = txtEmail.Text;

            Marketo_WS_2_0.ResultSyncLead  resultSyncLead = new Marketo_WS_2_0.ResultSyncLead();

            string result = SyncLead(leadEmail, ref resultSyncLead);
            #endregion
        }
        #endregion


        #region GetMultipleLeadsByDate
        private string GetMultipleLeadsByDate(string in_date, ref Marketo_WS_2_0.ResultGetMultipleLeads rs)
        {
            Marketo_WS_2_0.SuccessGetMultipleLeads response = new Marketo_WS_2_0.SuccessGetMultipleLeads();

            string results = "";

            string userID = txtUserID.Text;
            string EncryptionKey = txtEncryptionID.Text;


            string requestTimeStamp = ConvertDateToW3CTime(DateTime.Now);

            string stringToEncrypt = requestTimeStamp + userID;


            string message;
            string key;

            key = EncryptionKey;
            message = stringToEncrypt;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            string header = ByteToString(hashmessage);


            Marketo_WS_2_0.MktowsContextHeaderInfo ws_context = new Marketo_WS_2_0.MktowsContextHeaderInfo();
            ws_context = null;

            Marketo_WS_2_0.AuthenticationHeaderInfo ws_header = new Marketo_WS_2_0.AuthenticationHeaderInfo();
            ws_header.mktowsUserId = userID;
            ws_header.requestSignature = header.ToLower();
            ws_header.requestTimestamp = requestTimeStamp;

            Marketo_WS_2_0.ParamsGetMultipleLeads ws_leads = new Marketo_WS_2_0.ParamsGetMultipleLeads();
            ws_leads.lastUpdatedAt = System.DateTime.Parse(in_date);
            ws_leads.lastUpdatedAtSpecified = true;
            ws_leads.batchSize = 10;
            ws_leads.batchSizeSpecified = true;


            try
            {
                response = client.getMultipleLeads(ws_header, ws_leads);
                results = "";
                rs = response.result;
            }
            catch (Exception e)
            {
                results = e.Message;
                rs = null;
            }


            return results;
        }
        #endregion 

        
        #region GetMultipleLeadsByList
        private string GetMultipleLeadsByList(string in_list, ref Marketo_WS_2_0.ResultGetMultipleLeads rs)
        {
            Marketo_WS_2_0.SuccessGetMultipleLeads response = new Marketo_WS_2_0.SuccessGetMultipleLeads();

            string results = "";

            string userID = txtUserID.Text;
            string EncryptionKey = txtEncryptionID.Text;


            string requestTimeStamp = ConvertDateToW3CTime(DateTime.Now);

            string stringToEncrypt = requestTimeStamp + userID;


            string message;
            string key;

            key = EncryptionKey;
            message = stringToEncrypt;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            string header = ByteToString(hashmessage);


            Marketo_WS_2_0.MktowsContextHeaderInfo ws_context = new Marketo_WS_2_0.MktowsContextHeaderInfo();
            ws_context = null;

            Marketo_WS_2_0.AuthenticationHeaderInfo ws_header = new Marketo_WS_2_0.AuthenticationHeaderInfo();
            ws_header.mktowsUserId = userID;
            ws_header.requestSignature = header.ToLower();
            ws_header.requestTimestamp = requestTimeStamp;

            Marketo_WS_2_0.ParamsGetMultipleLeads ws_leads = new Marketo_WS_2_0.ParamsGetMultipleLeads();
            

            Marketo_WS_2_0.StaticListSelector sls = new Marketo_WS_2_0.StaticListSelector();
            sls.staticListName = in_list;
            //sls.staticListName = "#ST3345B2";
            sls.staticListIdSpecified = false;

            
            ws_leads.leadSelector= sls;

            ws_leads.batchSize = 10;
            ws_leads.batchSizeSpecified = true;

            
            try
            {
                response = client.getMultipleLeads(ws_header, ws_leads);
                results = "";
                rs = response.result;
            }
            catch (Exception e)
            {
                results = e.Message;
                rs = null;
            }


            return results;
        }
        #endregion 

       
        #region GetMultipleLeadsByListID
        private string GetMultipleLeadsByListID(string in_list, ref Marketo_WS_2_0.ResultGetMultipleLeads rs)
        {
            Marketo_WS_2_0.SuccessGetMultipleLeads response = new Marketo_WS_2_0.SuccessGetMultipleLeads();

            string results = "";

            string userID = txtUserID.Text;
            string EncryptionKey = txtEncryptionID.Text;


            string requestTimeStamp = ConvertDateToW3CTime(DateTime.Now);

            string stringToEncrypt = requestTimeStamp + userID;


            string message;
            string key;

            key = EncryptionKey;
            message = stringToEncrypt;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            string header = ByteToString(hashmessage);


            Marketo_WS_2_0.MktowsContextHeaderInfo ws_context = new Marketo_WS_2_0.MktowsContextHeaderInfo();
            ws_context = null;

            Marketo_WS_2_0.AuthenticationHeaderInfo ws_header = new Marketo_WS_2_0.AuthenticationHeaderInfo();
            ws_header.mktowsUserId = userID;
            ws_header.requestSignature = header.ToLower();
            ws_header.requestTimestamp = requestTimeStamp;

            Marketo_WS_2_0.ParamsGetMultipleLeads ws_leads = new Marketo_WS_2_0.ParamsGetMultipleLeads();




            Marketo_WS_2_0.LeadKeySelector lks = new Marketo_WS_2_0.LeadKeySelector();
            lks.keyType = Marketo_WS_2_0.LeadKeyRef.IDNUM;


            lks.keyValues = in_list.Split(',');


            ws_leads.leadSelector = lks;

            ws_leads.batchSize = 10;
            ws_leads.batchSizeSpecified = true;


            try
            {
                response = client.getMultipleLeads(ws_header, ws_leads);
                results = "";
                rs = response.result;
            }
            catch (Exception e)
            {
                results = e.Message;
                rs = null;
            }


            return results;
        }
        #endregion 

       

        #region Sync Lead
        private string SyncLead(string email, ref Marketo_WS_2_0.ResultSyncLead rs)
        {
            Marketo_WS_2_0.SuccessSyncLead response = new Marketo_WS_2_0.SuccessSyncLead();

            string results = "";

            string userID = txtUserID.Text;
            string EncryptionKey = txtEncryptionID.Text;



            string requestTimeStamp = ConvertDateToW3CTime(DateTime.Now);

            string stringToEncrypt = requestTimeStamp + userID;


            string message;
            string key;

            key = EncryptionKey;
            message = stringToEncrypt;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            string header = ByteToString(hashmessage);


            Marketo_WS_2_0.MktowsContextHeaderInfo ws_context = new Marketo_WS_2_0.MktowsContextHeaderInfo();
            ws_context = null;

            Marketo_WS_2_0.AuthenticationHeaderInfo ws_header = new Marketo_WS_2_0.AuthenticationHeaderInfo();
            ws_header.mktowsUserId = userID;
            ws_header.requestSignature = header.ToLower();
            ws_header.requestTimestamp = requestTimeStamp;

            Marketo_WS_2_0.ParamsSyncLead ws_lead = new Marketo_WS_2_0.ParamsSyncLead();
            Marketo_WS_2_0.LeadRecord LeadRec = new Marketo_WS_2_0.LeadRecord();
            
            LeadRec.Email = email;
            Marketo_WS_2_0.Attribute[] attribList = new Marketo_WS_2_0.Attribute[2];

            Marketo_WS_2_0.Attribute att1 = new Marketo_WS_2_0.Attribute();
            att1.attrName = "FirstName";
            att1.attrValue = "Joe";
            attribList[0] = att1;

            Marketo_WS_2_0.Attribute att2 = new Marketo_WS_2_0.Attribute();
            att2.attrName = "LastName";
            att2.attrValue = "Smith";
            attribList[1] = att2;

            LeadRec.leadAttributeList = attribList;

            ws_lead.leadRecord = LeadRec;
            LeadRec.Email = email;

            try
            {
                response = client.syncLead(ws_header, ws_context, ws_lead);
                results = "";
                rs = response.result;
            }
            catch (Exception e)
            {
                results = e.Message;
                rs = null;
            }


            return results;
        }
        #endregion

        #region Get Leads
        private string GetLeads(string LeadID, ref Marketo_WS_2_0.ResultGetLead rs)
        {
            Marketo_WS_2_0.SuccessGetLead response = new Marketo_WS_2_0.SuccessGetLead();

            string results = "";

            string userID = txtUserID.Text;
            string EncryptionKey = txtEncryptionID.Text;



            string requestTimeStamp = ConvertDateToW3CTime(DateTime.Now);

            string stringToEncrypt = requestTimeStamp + userID;


            string message;
            string key;

            key = EncryptionKey;
            message = stringToEncrypt;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            string header = ByteToString(hashmessage);



            Marketo_WS_2_0.AuthenticationHeaderInfo ws_header = new Marketo_WS_2_0.AuthenticationHeaderInfo();
            ws_header.mktowsUserId = userID;
            ws_header.requestSignature = header.ToLower();
            ws_header.requestTimestamp = requestTimeStamp;

            Marketo_WS_2_0.ParamsGetLead ws_lead = new Marketo_WS_2_0.ParamsGetLead();
            Marketo_WS_2_0.LeadKey ws_leadkey = new Marketo_WS_2_0.LeadKey();
            ws_leadkey.keyType = Marketo_WS_2_0.LeadKeyRef.IDNUM;
            ws_leadkey.keyValue = LeadID;
            ws_lead.leadKey = ws_leadkey;



            try
            {
                response = client.getLead(ws_header, ws_lead);
                results = "";
                rs = response.result;
            }
            catch (Exception e)
            {
                results = e.Message;
                rs = null;
            }


            return results;
        }
        #endregion

        #region Request Campaign
        private string RequestCampaign(string MarketoID,string campaignID, ref Marketo_WS_2_0.ResultRequestCampaign rs)
        {
            Marketo_WS_2_0.SuccessRequestCampaign response = new Marketo_WS_2_0.SuccessRequestCampaign();

            string results = "";

            string userID = txtUserID.Text;
            string EncryptionKey = txtEncryptionID.Text;



            string requestTimeStamp = ConvertDateToW3CTime(DateTime.Now);

            string stringToEncrypt = requestTimeStamp + userID;


            string message;
            string key;

            key = EncryptionKey;
            message = stringToEncrypt;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            string header = ByteToString(hashmessage);


            Marketo_WS_2_0.MktowsContextHeaderInfo ws_context = new Marketo_WS_2_0.MktowsContextHeaderInfo();
            ws_context = null;

            Marketo_WS_2_0.AuthenticationHeaderInfo ws_header = new Marketo_WS_2_0.AuthenticationHeaderInfo();
            ws_header.mktowsUserId = userID;
            ws_header.requestSignature = header.ToLower();
            ws_header.requestTimestamp = requestTimeStamp;

            
            Marketo_WS_2_0.ParamsRequestCampaign reqCampaign = new Marketo_WS_2_0.ParamsRequestCampaign();

            reqCampaign.source = Marketo_WS_2_0.ReqCampSourceType.MKTOWS;

            reqCampaign.campaignId = int.Parse(campaignID);

            
            Marketo_WS_2_0.LeadKey[] leadList = new Marketo_WS_2_0.LeadKey[1];


            Marketo_WS_2_0.LeadKey lk = new Marketo_WS_2_0.LeadKey();

            lk.keyType = Marketo_WS_2_0.LeadKeyRef.IDNUM;
            lk.keyValue = MarketoID;

            leadList[0] = lk;

            reqCampaign.leadList = leadList;


            try
            {
                response = client.requestCampaign(ws_header, reqCampaign);
                results = "";
                rs = response.result;
            }
            catch (Exception e)
            {
                results = e.Message;
                rs = null;
            }


            return results;
        }
        #endregion

        #region supporting Functions
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }


        public static string HashCode(string str)
        {
            string rethash = "";
            try
            {

                System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
                System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                string strerr = "Error in HashCode : " + ex.Message;
            }
            return rethash;
        }


        public static string ConvertDateToW3CTime(DateTime date)
        {
            var utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(date);
            string w3CTime = date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
            w3CTime += "Z";
            //w3CTime += utcOffset == TimeSpan.Zero ? "Z" :
            //    String.Format("{0}{1:00}:{2:00}", (utcOffset > TimeSpan.Zero ? "+" : "-")
            //    , Math.Abs(utcOffset.Hours), utcOffset.Minutes);

            //w3CTime += utcOffset == TimeSpan.Zero ? "Z" :
            //    String.Format("{0}{1:00}:{2:00}", (utcOffset > TimeSpan.Zero ? "+" : "-")
            //    , utcOffset.Hours, utcOffset.Minutes);
            return w3CTime;
        }
        #endregion

      

        


    }
}