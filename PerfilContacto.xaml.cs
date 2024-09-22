using Microsoft.Maui.Controls;
using static PContactos.App;
using The49.Maui.BottomSheet;
using PContactos.Model;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using PartialMethodsPhoneDialer = InvocarCodigoPlataforma.Servicios.MetodosParciales.PhoneDialer;



namespace PContactos;

public partial class PerfilContacto : ContentPage
{
    private ObservableCollection<ContactItem> contacts;
    public PerfilContacto(ObservableCollection<ContactItem> contacts)


    {
        InitializeComponent();
        this.contacts = contacts;
        // Establecer el contexto de enlace a la variable seleccionada
        BindingContext = App.selectedContact; // Asegúrate de que esto sea correcto

        // Asignar los íconos
        Call.Text = "\uf095"; // Ícono de llamada
        Message.Text = "\uf4a6"; // Ícono de mensaje
        phoneIcon.Text = "\uf095"; // Ícono de teléfono
        cityIcon.Text = "\uf64f"; // Ícono de ciudad
        mailIcon.Text = "\uf0e0"; // Ícono de correo electrónico

        // Cargar detalles del contacto
        LoadContactDetails();
    }

    private void LoadContactDetails()
    {
        var contact = App.selectedContact; // Obtener el contacto seleccionado
        if (contact != null)
        {
            // Asigna los valores a los elementos de la UI
            name.Text = contact.Name;
            occupation.Text = contact.Occupation;
            phoneNumber.Text = contact.PhoneNumber;
            address.Text = contact.Address;
            email.Text = contact.Email;

            // Carga la imagen si es necesario
            contactImage.Source = contact.ContactPicture; // Ajusta según tu UI
        }
        else
        {
            // Maneja el caso donde no hay contacto seleccionado
            DisplayAlert("Error", "No se encontró el contacto.", "OK");
        }
    }


    private async void LlamarClick(object sender, EventArgs e)
    {
        // Solicita los permisos para hacer llamadas telefónicas
        await RequestPhonePermission();
    }

    async Task RequestPhonePermission()
    {
        // Verifica si la plataforma es Android, si no, retorna sin hacer nada
        if (DeviceInfo.Platform != DevicePlatform.Android)
            return;

        var status = PermissionStatus.Unknown;

        // Verifica el estado actual de los permisos para usar el teléfono
        status = await Permissions.CheckStatusAsync<Permissions.Phone>();

        // Si los permisos ya fueron concedidos, realiza la llamada
        if (status == PermissionStatus.Granted)
        {
            var dialer = new PartialMethodsPhoneDialer();
            dialer.LlamarTelefono(selectedContact.PhoneNumber); // Llama al número seleccionado
            return;
        }

        // Si se debe mostrar una justificación para la solicitud de permisos
        if (Permissions.ShouldShowRationale<Permissions.Phone>())
        {
            await Shell.Current.DisplayAlert("Permisos necesarios", "BECAUSE!!!", "OK");
        }

        // Solicita los permisos al usuario
        status = await Permissions.RequestAsync<Permissions.Phone>();

        // Si los permisos no fueron concedidos, muestra una alerta informativa
        if (status != PermissionStatus.Granted)
        {
            await Shell.Current.DisplayAlert("Permiso requerido",
                "Se requiere permiso para realizar llamadas telefónicas. " +
                "Este es solo un test.", "OK");
        }
        // Si los permisos fueron concedidos, realiza la llamada
        else if (status == PermissionStatus.Granted)
        {
            var dialer = new PartialMethodsPhoneDialer();
            dialer.LlamarTelefono(selectedContact.PhoneNumber); // Llama al número seleccionado
        }
    }


    private async void Mensaje(object sender, EventArgs e)
    {
        if (Sms.Default.IsComposeSupported)
        {
            string[] recipients = new[] { selectedContact.PhoneNumber };
            string text = "";

            var message = new SmsMessage(text, recipients);

            await Sms.Default.ComposeAsync(message);
        }
    }

    private async void Deletee(object sender, EventArgs e)
    {
        // Muestra una alerta de confirmación antes de eliminar
        bool isConfirmed = await Shell.Current.DisplayAlert(
            "Confirmación",
            "¿Estás seguro de que deseas eliminar este contacto?",
            "Sí",
            "No");

        // Si el usuario confirma, se procede a eliminar el contacto
        if (isConfirmed)
        {
            contacts.Remove(App.selectedContact);

            // Navega hacia atrás en la pila de navegación
            await Navigation.PopAsync();

            // Muestra un mensaje toast
            var toast = Toast.Make("Eliminó contacto.");
            await toast.Show();
        }
        else
        {
            // Si el usuario cancela, muestra un mensaje opcional o no hace nada
            var toast = Toast.Make("Canceló la eliminación.");
            await toast.Show();
        }
    }


    private async void Editar(object sender, EventArgs e)
    {
        var pagina = new ActualizarContacto();
        pagina.CornerRadius = 20;
        pagina.HasBackdrop = true;
        await pagina.ShowAsync(Window);


    }

    private async void aggImagen(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Selecciona una nueva imagen de perfil"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                contactImage.Source = ImageSource.FromStream(() => stream);

                // Actualiza la imagen en el modelo de contactos
                var contact = App.selectedContact;
                if (contact != null)
                {
                    contact.ContactPicture = result.FullPath; // Guarda la ruta de la imagen en el objeto ContactItem
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al seleccionar la imagen: {ex.Message}", "OK");
        }
    }
}

