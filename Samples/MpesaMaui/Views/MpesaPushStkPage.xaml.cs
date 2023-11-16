using MpesaMaui.ViewModels;

namespace MpesaMaui.Views;

public partial class MpesaPushStkPage : ContentPage
{
	public MpesaPushStkPage(MpesaPushStkViewModel mpesaPushStkViewModel)
	{
		BindingContext = mpesaPushStkViewModel;
		InitializeComponent();
	}
}