using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace BookLibrary.Main
{
    public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
    {
        private static ResourceManifest manifest;

        static ResourceManagementOptionsConfiguration()
        {
            manifest = new ResourceManifest();

            manifest
                .DefineStyle("MaterialTheme")
                .SetUrl("~/BookLibrary.Main/classic/resources/BookLibrary.UI.Books-all.css");

            manifest
                .DefineScript("ExtJs")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/ext-all.js");

            manifest
                .DefineScript("Books.controller.BookAuthorsGridController")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/controller/BookAuthorsGridController.js");
            manifest
                .DefineScript("Books.controller.BookController")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/controller/BookController.js");
            manifest
                .DefineScript("Books.controller.BooksGridController")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/controller/BooksGridController.js");

            manifest
                .DefineScript("Books.model.Base")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/model/Base.js");
            manifest
                .DefineScript("Books.model.Book")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/model/Book.js");

            manifest
                .DefineScript("Books.service.BookService")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/service/BookService.js");
            manifest
                .DefineScript("Books.service.BookTracksService")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/service/BookTracksService.js");
            manifest
                .DefineScript("Books.service.UserService")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/service/UserService.js");

            manifest
                .DefineScript("Books.validator.BookNameValidator")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/validator/BookNameValidator.js");
            manifest
                .DefineScript("Books.validator.BookYearValidator")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/validator/BookYearValidator.js");

            manifest
                .DefineScript("Books.view.window.BookWindow")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/view/window/BookWindow.js");

            manifest
                .DefineScript("Books.view.BookAuthorsGrid")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/view/BookAuthorsGrid.js");
            manifest
                .DefineScript("Books.view.BooksGrid")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/view/BooksGrid.js");
            manifest
                .DefineScript("Books.view.BookTracksGrid")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/view/BookTracksGrid.js");

            manifest
                .DefineScript("Books.viewmodel.BooksViewModel")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/viewmodel/BooksViewModel.js");
            manifest
                .DefineScript("Books.viewmodel.BookViewModel")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/viewmodel/BookViewModel.js");

            manifest
                .DefineScript("Books.Constants")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/Constants.js");

            manifest
                .DefineScript("Books.BooksApplication")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJs/Books/BooksApplication.js");

        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(manifest);
        }
    }
}
