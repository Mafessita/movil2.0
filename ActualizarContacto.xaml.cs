
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using static PContactos.App;
using PContactos.Model;

namespace PContactos;

public partial class ActualizarContacto 
{
	public ActualizarContacto()
	{
		InitializeComponent();


        personIcon.Text = "\uf007";
        jobIcon.Text = "\ue0c8";

        phoneIcon.Text = "\uf095";

        cityIcon.Text = "\uf64f";

        mailIcon.Text = "\uf0e0";

        contactName.Text = selectedContact.Name;           
        contactOccupation.Text = selectedContact.Occupation;   
        contactPhone.Text = selectedContact.PhoneNumber;   
        contactAddress.Text = selectedContact.Address;     
        contactEmail.Text = selectedContact.Email;         


    }


    private async void CancelBotton_Clicked(object sender, EventArgs e)
    {
        SetFieldsEnabled(false); // Desactiva los campos
        await DismissAsync(); // Cierra la página actual
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        if (ValidateEntries())
        {
            // Actualiza los datos del contacto seleccionado
            selectedContact.Name = contactName.Text;
            selectedContact.Occupation = contactOccupation.Text;
            selectedContact.PhoneNumber = contactPhone.Text;
            selectedContact.Address = contactAddress.Text;
            selectedContact.Email = contactEmail.Text;

       

            SetFieldsEnabled(false);

            var toast = Toast.Make("Contacto Actualizado.");
            await toast.Show();
            await DismissAsync();
        }
    }

    private bool ValidateEntries()
    {
        // Valida el nombre
        nameRequired.IsVisible = string.IsNullOrWhiteSpace(contactName.Text);

        // Valida el número de teléfono
        phoneRequired.IsVisible = string.IsNullOrWhiteSpace(contactPhone.Text) || contactPhone.Text.Length < 3;

        // Retorna verdadero si ambas validaciones son correctas
        return !nameRequired.IsVisible && !phoneRequired.IsVisible;
    }


    // Método para habilitar/deshabilitar los campos
    private void SetFieldsEnabled(bool isEnabled)
    {
        contactName.IsEnabled = isEnabled;
        contactOccupation.IsEnabled = isEnabled;
        contactPhone.IsEnabled = isEnabled;
        contactAddress.IsEnabled = isEnabled;
        contactEmail.IsEnabled = isEnabled;
    }


}