using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Download;

namespace UploadToGoogleDrive
{
    public partial class Index : System.Web.UI.Page
    {
        public String AppName = "My Project";
        private DriveService Service = new DriveService();
        public static string[] Scopes = { DriveService.Scope.Drive };
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        string[] extnFiles = { ".pdf", ".doc", ".docx", ".xlsx" };

        protected void BtnAddToDrive_Click(object sender, EventArgs e)
        {
           
            try
            {

                if (fileUpload.HasFile)
                {
                    DriveService service = GetService();
                    //check file exten.
                    string fileExtension = System.IO.Path.GetExtension(fileUpload.FileName);
                    if (extnFiles.Contains(fileExtension.ToLower()) == false)
                    {
                        lblOutput.Text = "extension file not allowed please select file with extenstion:pdf,doc,docx,xlsx";
                        //If you want to alert //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('extension file not allowed please select file with extenstion:pdf,doc,docx,xlsx')", true);

                    }
                    else
                    {
                        //check file size.
                        int fileSize = fileUpload.PostedFile.ContentLength / 1024;
                        if (fileSize > 2048)//allowed only less than 2MG 
                        {
                            lblOutput.Text = "size large please select file with size less than 2MG";
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('size large please select file with size less than 2MG')", true);
                        }
                        else
                        {
                            var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                            FileMetaData.Name = Path.GetFileName(fileUpload.FileName);
                            //FileMetaData.MimeType = MimeMapping.GetMimeMapping(path);

                            FilesResource.CreateMediaUpload request;
                            request = service.Files.Create(FileMetaData, fileUpload.FileContent, FileMetaData.MimeType);

                            request.Fields = "id";
                            request.Upload();
                            lblOutput.Text = "File "+ Path.GetFileName(fileUpload.FileName)+ " Uploaded successfully.";
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('File" + Path.GetFileName(fileUpload.FileName) + "Uploaded successfully..')", true);

                        }
                    }
                }
                else
                {
                    //If User did not select file
                    lblOutput.Text = "please upload file";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('please upload file')", true);

                }
            }
            catch (Exception ex)
            {
                lblOutput.Text = "somthing went wrong during upload file "+ex.Message;

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('somthing went wrong during upload file" + ex.Message + "')", true);
            }
        }
        //create Drive API service.
        public DriveService GetService()
        {
            //get Credentials from client_secret.json file 
            UserCredential credential;
            using (var stream = new FileStream(@"D:\client_secret2.json", FileMode.Open, FileAccess.Read))
            {
                String FolderPath = @"D:\";
                String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials1.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }

            //create Drive API service.
            Service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName,
            });
            return Service;
        }

    }
    
}