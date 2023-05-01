Ext.define('BookLibrary.UI.Books.viewmodel.BookViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.bookviewmodel',
    stores: {
        bookAuthorsStore: {
            fields: ['name'],
            data: []
        }
    },
    data: {
        book: Ext.create('model.book').data,
        bookTracks: [],
        readOnly: false,
        bookAvailableReadOnly: false,
        isNew: true,
        authorsSelection: null
    }
});