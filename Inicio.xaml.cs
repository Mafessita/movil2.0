namespace PContactos;
using System.Collections.ObjectModel;
using static PContactos.App;
using System.Threading.Tasks;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.ApplicationModel.Communication;
using PContactos.Model;

public partial class Inicio : ContentPage

{
    public ObservableCollection<ContactItem> contacts { get; private set; } // ObservableCollection para almacenar los contactos

    public Inicio()
    {
        InitializeComponent();
        contacts = new ObservableCollection<ContactItem>(); // Inicializa la colección
        contactsList.ItemsSource = contacts; // Asigna la colección a la CollectionView
    }

    void addContactButton_Clicked(object sender, EventArgs e)
    {
        var pagina = new NewContact();
        pagina.CornerRadius = 20;
        pagina.HasBackdrop = true;
        pagina.ContactAdded += OnContactAdded; // Suscribirse al evento ContactAdded
        pagina.ShowAsync(Window); // Mostrar el BottomSheet
    }

    private void OnContactAdded(ContactItem newContact)
    {
        contacts.Add(newContact); // Agregar el nuevo contacto a la lista
    }

    private async void SeleccionarContacto(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {

            var selectedItem = e.CurrentSelection.FirstOrDefault() as ContactItem;
            App.selectedContact = selectedItem;
            contactsList.SelectedItem = null;

            try
            {
                await Navigation.PushAsync(new PerfilContacto(contacts));

            }

            catch (Exception ex)
            {
                // Registra el error o muestra un mensaje
                Console.WriteLine($"Error al cargar el perfil: {ex.Message}");
                // Opcionalmente, puedes mostrar un mensaje al usuario
                await DisplayAlert("Error", "No se pudo cargar el perfil del contacto.", "OK");
            }



        }
    }
    private void Buscar(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            // Si el texto de búsqueda está vacío, muestra todos los contactos
            contactsList.ItemsSource = contacts;
        }
        else
        {
            // Filtra los contactos basados en el texto de búsqueda
            contactsList.ItemsSource = contacts.Where(contact =>
                contact.Name.ToLower().Contains(e.NewTextValue.ToLower()) ||        // Filtro por el nombre
                contact.PhoneNumber.ToLower().Contains(e.NewTextValue.ToLower())    // Filtro por el número de teléfono
            ).ToList(); // Convierte el resultado en una lista
        }
    }
}

