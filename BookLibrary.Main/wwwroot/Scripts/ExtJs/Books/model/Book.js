Ext.define('BookLibrary.UI.Books.model.Book', {
    extend: 'BookLibrary.UI.Books.model.Base',
    alias: 'model.book',
    requires: [
        'BookLibrary.UI.Books.validator.BookNameValidator',
        'BookLibrary.UI.Books.validator.BookYearValidator'
    ],
    fields: [
        { name: 'id', defaultValue: null },
        { name: 'name', defaultValue: null },
        { name: 'authors', defaultValue: [] },
        { name: 'year', type: 'date', defaultValue: null },
        { name: 'available', type: 'bool', defaultValue: true }
    ],
    idProperty: 'id',
    validators: {
        name: Ext.create('BookLibrary.UI.Books.validator.BookNameValidator'),
        year: Ext.create('BookLibrary.UI.Books.validator.BookYearValidator')
    }
});