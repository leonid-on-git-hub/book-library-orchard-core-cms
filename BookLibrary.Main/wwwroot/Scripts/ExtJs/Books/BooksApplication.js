Ext.require([
    'BookLibrary.UI.Books.*'
]);

Ext.onReady(function () {
    Ext.Ajax.on('beforerequest', function (conn, options, eOpts) {
        options.headers = {
            'RequestVerificationToken': Constants.getRequestVerificationToken()
        };
    }, this);
    Ext.create('BookLibrary.UI.Books.view.BooksGrid');
});