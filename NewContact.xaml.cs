using Microsoft.Maui.Controls;
using The49.Maui.BottomSheet;
using CommunityToolkit.Maui.Alerts;
using static PContactos.App;
using PContactos.Model;

namespace PContactos;

public partial class NewContact : BottomSheet
{
    public event Action<ContactItem> ContactAdded; // Definir el evento
    bool lessInfoVar = false;
    public NewContact()
    {
        InitializeComponent();

        Closebtn.Text = "\uf00d";
        Closebtn.FontFamily = "fa-regular-400";

        Checkbtn.Text = "\uf00c";
        Checkbtn.FontFamily = "fa-regular-400";

        personIcon.Text = "\uf007";
        jobIcon.Text = "\ue0c8";
        phoneIcon.Text = "\uf095";
        cityIcon.Text = "\uf64f";
        mailIcon.Text = "\uf0e0";
    }

    private async void CerrarVentana(object sender, EventArgs e)
    {
        await DismissAsync(); // Cierra el BottomSheet
    }

    private async void EnviarDatos(object sender, EventArgs e)
    {
        if (validateEntrys())
        {
            var newContact = new ContactItem
            {
                Name = contactName.Text,
                Occupation = contactOccupation.Text,
                PhoneNumber = contactPhone.Text,
                Address = contactAddress.Text,
                Email = contactEmail.Text,
                ContactPicture = "defaultuser.jpg" // O la ruta de la imagen
            };

            ContactAdded?.Invoke(newContact); // Disparar el evento

            var toast = Toast.Make("Contacto agregado.");
            await toast.Show();
            await DismissAsync();
        }
    }

    private bool validateEntrys() // Método de validación
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(contactName.Text))
        {
            isValid = false;
            // Aquí puedes mostrar un mensaje de error
        }

        if (string.IsNullOrWhiteSpace(contactPhone.Text))
        {
            isValid = false;
            // Aquí puedes mostrar un mensaje de error
        }

        return isValid;
    }


    private void lessInfo_Clicked(object sender, EventArgs e)
    {
        if (lessInfoVar == false)
        {
            lessInfoVar = true;
            lessInfo.Text = "Add more info";
            principalStack.HeightRequest = 250;
            occupationStack.IsVisible = false;
            addressStack.IsVisible = false;
            emailStack.IsVisible = false;

            contactOccupation.Text = "";
            contactAddress.Text = "";
            contactEmail.Text = "";

        }
        else
        {


            lessInfoVar = false;
            lessInfo.Text = "Add less info";
            principalStack.HeightRequest = 490;
            occupationStack.IsVisible = true;
            addressStack.IsVisible = true;
            emailStack.IsVisible = true;
        }
    }
}
