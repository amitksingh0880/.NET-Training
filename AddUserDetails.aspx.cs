
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace CountryCodeApplication
{
    public partial class AddUserDetails : CountryCodeApplication.AuthorizationRoute
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["MyConnection"]?.ConnectionString;
         int userId;
        private readonly string imageFolder = "~/Uploads/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userIdStr = Request.QueryString["userId"];
                if (!int.TryParse(userIdStr, out userId))
                {
                    lblMessage.Text = "Invalid or missing User ID.";
                    return;
                }

                ViewState["UserId"] = userId;
                LoadUserDetails(userId);
            }
        }

        private void LoadUserDetails(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                SELECT u.Username, up.FullName, up.Address, up.PhoneNumber, up.Gender, up.ProfilePicture
                FROM AppUsers u
                LEFT JOIN UserProfile up ON u.Id = up.UserId
                WHERE u.Id = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtUsername.Text = reader["Username"]?.ToString();
                    txtFullName.Text = reader["FullName"]?.ToString();
                    txtAddress.Text = reader["Address"]?.ToString();
                    txtPhoneNumber.Text = reader["PhoneNumber"]?.ToString();
                    txtGender.Text = reader["Gender"]?.ToString();

                    string profileImage = reader["ProfilePicture"]?.ToString();
                    if (!string.IsNullOrEmpty(profileImage))
                    {
                        imgProfile.ImageUrl = profileImage;
                        imgProfile.Visible = true;
                    }
                }
                else
                {
                    lblMessage.Text = "User not found.";
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            userId = (int)ViewState["UserId"];
            string profileImagePath = imgProfile.ImageUrl;

            // Handle image upload
            if (fuProfileImage.HasFile)
            {
                try
                {
                    string fileExtension = Path.GetExtension(fuProfileImage.FileName).ToLower();
                    if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                    {
                        string fileName = $"{userId}_{Guid.NewGuid()}{fileExtension}";
                        string savePath = Server.MapPath(imageFolder + fileName);
                        fuProfileImage.SaveAs(savePath);
                        profileImagePath = imageFolder + fileName;
                    }
                    else
                    {
                        lblMessage.Text = "Only JPG, JPEG, or PNG files are allowed.";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error uploading image: " + ex.Message;
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Update AppUsers table
                SqlCommand cmd1 = new SqlCommand("UPDATE AppUsers SET Username = @Username WHERE Id = @UserId", conn);
                cmd1.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                cmd1.Parameters.AddWithValue("@UserId", userId);
                cmd1.ExecuteNonQuery();

                // Update or Insert UserProfile
                SqlCommand cmd2 = new SqlCommand(@"
                IF EXISTS (SELECT 1 FROM UserProfile WHERE UserId = @UserId)
                    UPDATE UserProfile 
                    SET FullName = @FullName, Address = @Address, PhoneNumber = @PhoneNumber, Gender = @Gender, ProfilePicture = @ProfilePicture
                    WHERE UserId = @UserId
                ELSE
                    INSERT INTO UserProfile (UserId, FullName, Address, PhoneNumber, Gender, ProfilePicture)
                    VALUES (@UserId, @FullName, @Address, @PhoneNumber, @Gender, @ProfilePicture)", conn);

                cmd2.Parameters.AddWithValue("@UserId", userId);
                cmd2.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd2.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                cmd2.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text.Trim());
                cmd2.Parameters.AddWithValue("@Gender", txtGender.Text.Trim());
                cmd2.Parameters.AddWithValue("@ProfilePicture", (object)profileImagePath ?? DBNull.Value);

                cmd2.ExecuteNonQuery();

                lblMessage.Text = "User details updated successfully.";
                imgProfile.ImageUrl = profileImagePath;
                imgProfile.Visible = !string.IsNullOrEmpty(profileImagePath);
            }
        }
    }
}
