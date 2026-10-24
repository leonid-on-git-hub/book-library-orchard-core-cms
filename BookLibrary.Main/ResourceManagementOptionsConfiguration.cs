using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace BookLibrary.Main
{
    public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
    {
        private static readonly ResourceManifest manifest;

        static ResourceManagementOptionsConfiguration()
        {
            manifest = new ResourceManifest();

            manifest
                .DefineStyle("TritonTheme")
                .SetUrl("~/BookLibrary.Main/classic/resources/BookLibrary.UI.Books-all.css");

            manifest
                .DefineStyle("ExtJS-patch")
                .SetUrl("~/BookLibrary.Main/Styles/ExtJS-patch.css");

            manifest
                .DefineScript("ExtJS")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/ext-all.js");

            manifest
                .DefineScript("Books.controller.BookAuthorsGridController")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/controller/BookAuthorsGridController.js");
            manifest
                .DefineScript("Books.controller.BookController")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/controller/BookController.js");
            manifest
                .DefineScript("Books.controller.BooksGridController")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/controller/BooksGridController.js");

            manifest
                .DefineScript("Books.model.Base")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/model/Base.js");
            manifest
                .DefineScript("Books.model.Book")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/model/Book.js");

            manifest
                .DefineScript("Books.service.BookService")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/service/BookService.js");
            manifest
                .DefineScript("Books.service.BookTracksService")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/service/BookTracksService.js");
            manifest
                .DefineScript("Books.service.UserService")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/service/UserService.js");

            manifest
                .DefineScript("Books.validator.BookAuthorValidator")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/validator/BookAuthorValidator.js");
            manifest
                .DefineScript("Books.validator.BookNameValidator")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/validator/BookNameValidator.js");
            manifest
                .DefineScript("Books.validator.BookYearValidator")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/validator/BookYearValidator.js");

            manifest
                .DefineScript("Books.view.window.BookWindow")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/view/window/BookWindow.js");

            manifest
                .DefineScript("Books.view.BookAuthorsGrid")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/view/BookAuthorsGrid.js");
            manifest
                .DefineScript("Books.view.BooksGrid")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/view/BooksGrid.js");
            manifest
                .DefineScript("Books.view.BookTracksGrid")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/view/BookTracksGrid.js");

            manifest
                .DefineScript("Books.viewmodel.BooksViewModel")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/viewmodel/BooksViewModel.js");
            manifest
                .DefineScript("Books.viewmodel.BookViewModel")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/viewmodel/BookViewModel.js");

            manifest
                .DefineScript("Books.Constants")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/Constants.js");

            manifest
                .DefineScript("Books.BooksApplication")
                .SetUrl("~/BookLibrary.Main/Scripts/ExtJS/Books/BooksApplication.js");

        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(manifest);
        }
    }
}
