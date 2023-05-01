Ext.define('BookLibrary.UI.Books.service.BookService', {
    alternateClassName: 'BookService',
    singleton: true,
    requires: ['Ext.Ajax', 'Ext.JSON', 'Ext.Deferred', 'Ext.Promise'],

    getBook: function (bookId) {
        return new Ext.Promise(function (resolve, reject) {
            Ext.Ajax.request({
                url: Constants.APPLICATION_BASE_URL + 'api/books/' + bookId,
                method: 'GET',
                success: function (response) {
                    var book = Ext.decode(response.responseText);
                    resolve(book);
                },
                failure: function (response) {
                    reject(response.status);
                }
            });
        });
    },

    createBook: function (book) {
        book.id = null;
        return new Ext.Promise(function (resolve, reject) {
            Ext.Ajax.request({
                url: Constants.APPLICATION_BASE_URL + 'api/books',
                method: 'POST',
                jsonData: book,
                success: function (response) {
                    var createdBook = Ext.decode(response.responseText);
                    resolve(createdBook);
                },
                failure: function (response) {
                    reject(response.responseText);
                }
            });
        });
    },

    updateBook: function (book) {
        return new Ext.Promise(function (resolve, reject) {
            Ext.Ajax.request({
                url: Constants.APPLICATION_BASE_URL + 'api/books/' + book.id,
                method: 'PUT',
                jsonData: book,
                success: function (response) {
                    resolve(response);
                },
                failure: function (response) {
                    reject(response.responseText);
                }
            });
        });
    },

    deleteBook: function (bookId) {
        return new Ext.Promise(function (resolve, reject) {
            Ext.Ajax.request({
                url: Constants.APPLICATION_BASE_URL + 'api/books/' + bookId,
                method: 'DELETE',
                success: function (response) {
                    resolve(response);
                },
                failure: function (response) {
                    reject(response.responseText);
                }
            });
        });
    }
});