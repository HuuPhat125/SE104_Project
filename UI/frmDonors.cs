using BloodBankManagementSystem.BLL;
using BloodBankManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;


namespace BloodBankManagementSystem.UI
{
    public partial class frmDonors : Form
    {
        public event EventHandler DataChanged;
        public frmDonors()
        {
            InitializeComponent();
        }
        //Create object of Donor BLL and Donor DAL
        donorBLL d = new donorBLL();
        donorDAL dal = new donorDAL();
        userDAL udal = new userDAL();

        //Global Variable for Image
        string imageName = "no-image.jpg";
        string sourcePath = "";
        string destinationPath = "";

        string rowHeaderImage;

        private void frmDonors_Load(object sender, EventArgs e)
        {
            //Display Donors in DataGrid View
            DataTable dt = dal.Select();
            dgvDonors.DataSource = dt;

            //First we need to get the image Path
            string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length) - 10);

            string imagepath = path + "\\images\\no-image.jpg";

            //Display Image in PictureBox
            pictureBoxProfilePicture.Image = new Bitmap(imagepath);
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Close this form
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //We will write the code to Add new Donor
            //Step 1. Get the DAta from Manage Donors Form
            d.first_name = txtFirstName.Text;
            d.last_name = txtLastName.Text;
            d.email = txtEmail.Text;
            d.gender = cmbGender.Text;
            d.blood_group = cmbBloodGroup.Text;
            d.contact = txtContact.Text;
            d.address = txtAddress.Text;
            d.added_date = DateTime.Now;

            //Get The ID of Logged In USer
            string loggedInUser = frmLogin.loggedInUser;
            userBLL usr = udal.GetIDFromUsername(loggedInUser);

            d.added_by = usr.user_id;

            d.image_name = imageName;

            //Upload image
            //Check whether the user has selected the image or not
            if (imageName != "no-image.jpg")
            {
                //Upload the image
                File.Copy(sourcePath, destinationPath);
            }

            //Step2: Inserting Data into Database
            //Create a Boolean Variable to Isnert DAta into DAtabase and check whether the data inserted successfully of not
            bool isSuccess = dal.Insert(d);

            //if the Data is inserted successfully then the values of isSuccess will be True else it will be false
            if (isSuccess == true)
            {
                //Data Inserted Successfully
                MessageBox.Show("New Donor Added Successfully");

                //Refresh Datagrid View
                DataTable dt = dal.Select();
                dgvDonors.DataSource = dt;

                //Clear all the Textboxes
                Clear();
            }
            else
            {
                //Failed to Insert Data
                MessageBox.Show("Failed to Add new Donor.");
            }
        }

        //Create a Method to Clear all the Textboxes
        public void Clear()
        {
            //Clear all the TExtboxes
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtDonorID.Text = "";
            cmbGender.Text = "";
            cmbBloodGroup.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            imageName = "no-image.jpg";

            //Clear the PictureBox
            //First we need to get the image Path
            string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length) - 10);

            string imagepath = path + "\\images\\no-image.jpg";

            //Display Image in PictureBox
            pictureBoxProfilePicture.Image = new Bitmap(imagepath);
        }

        private void dgvDonors_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SElect the DAta from DAtagrid View and Display in our Form

            //1. Find the Row Selected
            int RowIndex = e.RowIndex;

            txtDonorID.Text = dgvDonors.Rows[RowIndex].Cells[0].Value.ToString();
            txtFirstName.Text = dgvDonors.Rows[RowIndex].Cells[1].Value.ToString();
            txtLastName.Text = dgvDonors.Rows[RowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvDonors.Rows[RowIndex].Cells[3].Value.ToString();
            txtContact.Text = dgvDonors.Rows[RowIndex].Cells[4].Value.ToString();
            cmbGender.Text = dgvDonors.Rows[RowIndex].Cells[5].Value.ToString();
            txtAddress.Text = dgvDonors.Rows[RowIndex].Cells[6].Value.ToString();
            cmbBloodGroup.Text = dgvDonors.Rows[RowIndex].Cells[7].Value.ToString();

            imageName = dgvDonors.Rows[RowIndex].Cells[9].Value.ToString();

            //Update the VAlue of rowHeaderImage
            rowHeaderImage = imageName;

            //Display The image of Selected Donor
            //Get the image path
            string paths = Application.StartupPath.Substring(0, (Application.StartupPath.Length) - 10);
            string imagePath = paths + "\\images\\" + imageName;
            //Display the Image of SElected User
            pictureBoxProfilePicture.Image = new Bitmap(imagePath);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Add the Functionality to Update the Donors
            //1. Get the Values from Form
            d.donor_id = int.Parse(txtDonorID.Text);
            d.first_name = txtFirstName.Text;
            d.last_name = txtLastName.Text;
            d.email = txtEmail.Text;
            d.gender = cmbGender.Text;
            d.blood_group = cmbBloodGroup.Text;
            d.contact = txtContact.Text;
            d.address = txtAddress.Text;
            //Get The ID of Logged In USer
            string loggedInUser = frmLogin.loggedInUser;
            userBLL usr = udal.GetIDFromUsername(loggedInUser);

            d.added_by = usr.user_id;
            d.image_name = imageName;

            // Upload new image
            //Check whether the user has selected the image or not
            if (imageName != "no-image.jpg")
            {
                //Upload the image
                File.Copy(sourcePath, destinationPath);
            }

            //Create a Boolean Variable to Check whether the data updated successfully or not
            bool isSuccess = dal.Update(d);

            //REmove the previous image
            //Check whether the donor has profile picture or not
            // Check whether the donor has a profile picture or not
            if (rowHeaderImage != "no-image.jpg")
            {
                // Only runs if the user has an image
                // Get the Path to the root folder of the project
                string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length) - 10);

                // Get the Path of the image using Path.Combine
                string imagePath = Path.Combine(path, "images", rowHeaderImage);

                // Call Clear Function
                Clear();

                // Call Garbage Collection
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Delete the Physical image file of Donors
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            //If the data updated successfully then the value of isSuccess will be true else it will be false
            if (isSuccess == true)
            {
                //Donor Updated Successfully
                MessageBox.Show("Donor updated Successfully.");
                Clear();

                //Refresh Datagridview
                DataTable dt = dal.Select();
                dgvDonors.DataSource = dt;

            }
            else
            {
                //Failed to Update
                MessageBox.Show("Failed to update donors.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get the value from form
            d.donor_id = int.Parse(txtDonorID.Text);

            // Check whether the donor has a profile picture or not
            if (rowHeaderImage != "no-image.jpg")
            {
                // Only runs if the user has an image
                // Get the Path to the root folder of the project
                string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length) - 10);

                // Get the Path of the image using Path.Combine
                string imagePath = Path.Combine(path, "images", rowHeaderImage);

                // Call Clear Function
                Clear();

                // Call Garbage Collection
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Delete the Physical image file of Donors
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            // Create a Boolean Variable to Check whether the donor deleted or not
            bool isSuccess = dal.Delete(d);

            if (isSuccess == true)
            {
                // Donor Deleted Successfully
                MessageBox.Show("Donor Deleted Successfully.");

                Clear();

                // Refresh Datagrid View
                DataTable dt = dal.Select();
                dgvDonors.DataSource = dt;
            }
            else
            {
                // Failed to Delete Donor
                MessageBox.Show("Failed to Delete Donor");
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            //Clear the TExtboxes
            Clear();
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            //Code to Select Image and Upload
            //1. Open the Dialog Box to SElect Image
            OpenFileDialog open = new OpenFileDialog();

            //2. FIlter the File Type (allow only image files)
            open.Filter = "Image Files Only (*.jpg; *.jpeg; *.png; *.gif| *.jpg; *.jpeg; *.png; *.gif)";

            //3. Check whether the image is selected or not
            if (open.ShowDialog() == DialogResult.OK)
            {
                //Check if the file exists or not
                if (open.CheckFileExists)
                {
                    //Display the Selected Image in PictureBox
                    pictureBoxProfilePicture.Image = new Bitmap(open.FileName);

                    //Rename the selected image 
                    //1. Get the Extension of SElected Image
                    string ext = Path.GetExtension(open.FileName);

                    string name = Path.GetFileNameWithoutExtension(open.FileName);

                    //Generate Random but Globally Unique Identifier
                    Guid g = new Guid();
                    g = Guid.NewGuid();

                    //Finally REname our Image
                    imageName = "Blood_Bank_MS_" + name + g + ext;

                    //Get the Source Path (Path of Image)
                    sourcePath = open.FileName;

                    //Get the Destination Path
                    string paths = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10);
                    //Path to Desitnation 
                    destinationPath = paths + "\\images\\" + imageName;

                    //Upload the Image to Destination Folder
                    //File.Copy(sourcePath, destinationPath);

                    //Display Message after the image is uploaded successfully
                    //MessageBox.Show("Image successfully uploaded.");
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Let's Add the Dunctionality to Search the Donors

            //1. Get the Keywords Typed on the Search TExt Box
            string keywords = txtSearch.Text;

            // Check Whether the Search TExtBox is Empty or Not
            if (keywords != null)
            {
                //Display the information of Donors Based on Keywords
                DataTable dt = dal.Search(keywords);
                dgvDonors.DataSource = dt;
            }
            else
            {
                //DIsplay all the Donors
                DataTable dt = dal.Select();
                dgvDonors.DataSource = dt;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void selectFile_Click(object sender, EventArgs e)
        {
            int ImportedRecord = 0, inValidItem = 0;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;

                try
                {

                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    using (var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath)))
                    {
                        // Assume the data is in the first worksheet
                        var worksheet = package.Workbook.Worksheets[0];

                        // Loop through all rows with data
                        for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                        {
                            // Read data from each column
                            d.first_name = worksheet.Cells[row, 1].Value?.ToString();
                            d.last_name = worksheet.Cells[row, 2].Value?.ToString();
                            d.email = worksheet.Cells[row, 3].Value?.ToString();
                            d.gender = worksheet.Cells[row, 4].Value?.ToString();
                            d.blood_group = worksheet.Cells[row, 5].Value?.ToString();
                            d.contact = worksheet.Cells[row, 6].Value?.ToString();
                            d.address = worksheet.Cells[row, 7].Value?.ToString();
                            d.added_date = DateTime.Now;

                            // Get the ID of the logged-in user
                            string loggedInUser = frmLogin.loggedInUser;
                            userBLL usr = udal.GetIDFromUsername(loggedInUser);
                            d.added_by = usr.user_id;

                            // Set the default image name
                            d.image_name = "no-image.jpg";

                            // Insert data into the database
                            bool isSuccess = dal.Insert(d);

                            if (isSuccess)
                            {
                                ImportedRecord++;
                            }
                            else
                            {
                                inValidItem++;
                            }
                        }

                        // Display a message with the result
                        MessageBox.Show($"Imported {ImportedRecord} records. {inValidItem} records failed to import.");

                        // Refresh the DataGridView
                        DataTable dt = dal.Select();
                        dgvDonors.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

    }
}
