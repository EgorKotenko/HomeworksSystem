using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Windows.Storage;
using Windows.Storage.Streams;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace diploma1
{
  public sealed partial class CoursesPage : Page
  {
    IMobileServiceTable<Courses> CoursesTable;

    public CoursesPage()
    {
      this.InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
      CoursesTable = App.MobileService.GetTable<Courses>();


      // add new courses

      /*await CoursesTable.InsertAsync(
         new Courses{Name = "алгебра (8 класс)"});
      await CoursesTable.InsertAsync(
       new Courses { Name = "ЕГЭ по математике (базовый уровень)" });
      await CoursesTable.InsertAsync(
           new Courses { Name = "геометрия (10 класс)" });
      await CoursesTable.InsertAsync(
           new Courses { Name = "ЕГЭ по математике (профильный уровень)" });
      await CoursesTable.InsertAsync(
         new Courses { Name = "ЕГЭ по информатике" });*/

      // add new homework

      /*IMobileServiceTable<Homeworks> HWs = App.MobileService.GetTable<Homeworks>();
      await HWs.InsertAsync(
         new Homeworks 
         {
             CourseID = "02715F5B-5B0F-443E-8BF8-8033CC8F575D",
             Number = 2,
             Date = DateTime.Today,
             Name = "простейшие текстовые задачи"
         }); */

      // add new problem

      /*IMobileServiceTable<Problems> Problems = App.MobileService.GetTable<Problems>();
      var image = await Helpers.LoadImage();
      if (image == null)
          return;

      await Problems.InsertAsync(
         new Problems
         {
             HomeworkID = "CEA3D3AE-FBD1-491C-976A-EB3E00AAC679",
             Number = 1,
             Statement = Convert.ToBase64String(await image.ToByteArray()),
             Answer = string.Format("49")
         });
      image = await Helpers.LoadImage();
      if (image == null)
          return;

      await Problems.InsertAsync(
         new Problems
         {
             HomeworkID = "CEA3D3AE-FBD1-491C-976A-EB3E00AAC679",
             Number = 2,
             Statement = Convert.ToBase64String(await image.ToByteArray()),
             Answer = string.Format("-0.1")
         });
      image = await Helpers.LoadImage();
      if (image == null)
          return;

      await Problems.InsertAsync(
         new Problems
         {
             HomeworkID = "CEA3D3AE-FBD1-491C-976A-EB3E00AAC679",
             Number = 3,
             Statement = Convert.ToBase64String(await image.ToByteArray()),
             Answer = string.Format("2")
         });
      image = await Helpers.LoadImage();
      if (image == null)
          return;

      await Problems.InsertAsync(
         new Problems
         {
             HomeworkID = "CEA3D3AE-FBD1-491C-976A-EB3E00AAC679",
             Number = 4,
             Statement = Convert.ToBase64String(await image.ToByteArray()),
             Answer = string.Format("2.16")
         });
      image = await Helpers.LoadImage();
      if (image == null)
          return;

      await Problems.InsertAsync(
         new Problems
         {
             HomeworkID = "CEA3D3AE-FBD1-491C-976A-EB3E00AAC679",
             Number = 5,
             Statement = Convert.ToBase64String(await image.ToByteArray()),
             Answer = string.Format("84")
         });*/


      CoursesList.ItemsSource = await CoursesTable.ToCollectionAsync();
    }


    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      (sender as Button).IsEnabled = false;

      var listCourses = CoursesList.ItemsSource as IEnumerable<Courses>;

      //  save to database
      foreach (var c in listCourses)
        await CoursesTable.UpdateAsync(c);

      this.Frame.Navigate(typeof(HomeworksPage));
    }
  }
}
