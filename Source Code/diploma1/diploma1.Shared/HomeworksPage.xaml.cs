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

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace diploma1
{
  public sealed partial class HomeworksPage : Page
  {
    class HomeworkViewModel
    {
      public Homeworks Homework { get; set; }

      public Courses Course { get; set; }

      public string Name1 { get { return string.Format("{0}", Course.Name); } }

      public string Name2 { get { return string.Format("дз {0}: {1} ({2:dd.MM.yyyy})", Homework.Number, Homework.Name, Homework.Date); } }
    }

    public HomeworksPage()
    {
      this.InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
      //    запросить только курсы из списка
      var CoursesSelected = await (from item in App.MobileService.GetTable<Courses>() where item.Selected select item).ToCollectionAsync();

      var Homeworks = App.MobileService.GetTable<Homeworks>();
      var HomeworksItems = new List<HomeworkViewModel>();
      foreach (var c in CoursesSelected)
      {
        var items = await (from h in Homeworks
                           where h.CourseID == c.Id
                           select new HomeworkViewModel { Homework = h, Course = c }).ToCollectionAsync();
        HomeworksItems.AddRange(items);
      }
      HomeworksList.ItemsSource = HomeworksItems;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {    
      this.Frame.GoBack();
    }


    private void ToProblemsPage(object sender, RoutedEventArgs e)
    {
      (sender as Button).IsEnabled = false;

      // передавать айди выбранного ДЗ
      this.Frame.Navigate(typeof(ProblemsListPage), ((sender as Grid).Tag as Homeworks).Id);
    }
  }
}
