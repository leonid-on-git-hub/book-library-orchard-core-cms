Ext.define('BookLibrary.UI.Books.viewmodel.BooksViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.booksviewmodel',
    stores: {
        booksPagedStore: {
            model: 'BookLibrary.UI.Books.model.Book',
            autoLoad: true,
            pageSize: 10,
            proxy: {
                type: 'rest',
                url: Constants.APPLICATION_BASE_URL + 'api/books',
                limitParam: 'count',
                startParam: 'from',
                headers: {
                    'RequestVerificationToken': Constants.getRequestVerificationToken()
                },
                extraParams: {
                    searchBookName: '{searchBookName}',
                    searchAuthor: '{searchAuthor}',
                    searchYear: '{searchYear}',
                    searchOnlyAvailable: '{searchOnlyAvailable}'
                },
                reader: {
                    type: 'json',
                    rootProperty: 'items',
                    totalProperty: 'totalCount'
                }
            }
        }
    },
    data: {
        searchBookName: '',
        searchAuthor: '',
        searchYear: null,
        searchOnlyAvailable: false,
        booksGridSelection: null
    }
});