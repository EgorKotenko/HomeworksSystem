using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.Http;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Popups;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace diploma1
{
  public sealed partial class ProblemsListPage : Page
  {

    class ProblemsViewModel : INotifyPropertyChanged
    {
      //  INotifyPropertyChanged
      public event PropertyChangedEventHandler PropertyChanged;
      protected void FirePropertyChanged(string name)
      {
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs(name));
      }

      public Problems Problem { get; set; }
      public string Name { get { return string.Format("задача № {0}", Problem.Number); } }



      public ProblemsViewModel(Problems p)
      {
        Problem = p;
        _AnswerHasFocus = false;
        _DontUpdateAnswer = false;
      }

      string _Answer;
      bool _AnswerHasFocus;
      bool _DontUpdateAnswer;
      public string Answer
      {
        get 
        { 
          return (Problem.Completed.HasValue && Problem.Completed.Value) 
                  ? Problem.Answer
                  : (_AnswerHasFocus) ? _Answer : "введите ответ"; 
        }
        set 
        { 
          if(!_DontUpdateAnswer)
            _Answer = value; 
        }
      }

      public async Task<bool> CheckAnswer()
      {
        //  если ответ еще не задан
        if (string.IsNullOrEmpty(_Answer))
          return false;

        bool result = _Answer.Equals(Problem.Answer, StringComparison.OrdinalIgnoreCase);
        if (result)
        {
          Problem.Completed = true;
          FirePropertyChanged("Color");
          FirePropertyChanged("CompletedColor");

          await App.MobileService.GetTable<Problems>().UpdateAsync(Problem);
        }
        return result;
      }

      public void AnswerFocusChanged(bool isFocused)
      {
        _AnswerHasFocus = isFocused;

        if (isFocused)
        {
          _DontUpdateAnswer = true;
          FirePropertyChanged("Answer");
          _DontUpdateAnswer = false;
        }
      }



      public string Done { get { return " зачтено"; } }
      public Brush CompletedColor { get { return new SolidColorBrush((!Problem.Completed.HasValue) ? Colors.Black : (Problem.Completed.Value) ? Colors.DeepSkyBlue : Colors.Black); } }
      public Brush Color { get { return new SolidColorBrush((Problem.Completed.HasValue && Problem.Completed.Value) ? Colors.Green : Colors.Silver); } }

      public BitmapImage Statement { get { return Helpers.LoadImage(Convert.FromBase64String(Problem.Statement)); } }

      public string Title { get { return string.Format("задача №{0}", Problem.Number); } }

      public ProblemsViewModel ThisModel { get { return this; } }


    }



    public ProblemsListPage()
    {
      this.InitializeComponent();
    }


    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
      var hwID = e.Parameter as string;
//       var Problems = App.MobileService.GetTable<Problems>();
//       ProblemsList.ItemsSource = await (from p in Problems
//                                         where hwID == p.HomeworkID
//                                         select new ProblemsViewModel { Problem = p }).ToCollectionAsync();

//       this.DataContext = new ProblemViewModel
//       {
//         Problem = await App.MobileService.InvokeApiAsync<Problems>("getproblem", HttpMethod.Get, new Dictionary<string, string>() { { "id", Id } })
//       };

      ProblemsList.ItemsSource = 
        (await App.MobileService.InvokeApiAsync<Problems[]>("getproblems", HttpMethod.Get, new Dictionary<string, string>() { { "id", hwID } })).
        Select((p) => new ProblemsViewModel(p));
    }


    async void CheckAnswerAndShowDialog(ProblemsViewModel view)
    {
      if (await view.CheckAnswer())
      {
        var dialog = new MessageDialog("задача решена верно.", "результат проверки");
        dialog.Commands.Add(new UICommand("закрыть"));
        await dialog.ShowAsync();
      }
      else
      {
        var dialog = new MessageDialog("задача решена неверно.", "результат проверки");
        dialog.Commands.Add(new UICommand("закрыть"));
        await dialog.ShowAsync();
      }
    }


    private async void ButtonCheck_Click(object sender, RoutedEventArgs e)
    {
      var button = sender as Button;
      var view = button.Tag as ProblemsViewModel;
      CheckAnswerAndShowDialog(view);
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
      this.Frame.GoBack();
    }


    private void TextBoxAnswer_GotFocus(object sender, RoutedEventArgs e)
    {
      var textbox = sender as TextBox;
      var view = textbox.Tag as ProblemsViewModel;
      view.AnswerFocusChanged(true);
    }


    private void TextBoxAnswer_LostFocus(object sender, RoutedEventArgs e)
    {
      var textbox = sender as TextBox;
      var view = textbox.Tag as ProblemsViewModel;
      view.AnswerFocusChanged(false);
    }

    private void TextBoxAnswer_KeyDown(object sender, KeyRoutedEventArgs e)
    {
      if (e.Key != Windows.System.VirtualKey.Enter)
        return;

      var textbox = sender as TextBox;
      var view = textbox.Tag as ProblemsViewModel;
      CheckAnswerAndShowDialog(view);
    }
  }
}
