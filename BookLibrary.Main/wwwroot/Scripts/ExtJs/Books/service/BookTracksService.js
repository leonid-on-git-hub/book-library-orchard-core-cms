Ext.define('BookLibrary.UI.Books.service.BookTracksService', {
    alternateClassName: 'BookTracksService',
    singleton: true,
    requires: ['Ext.Ajax', 'Ext.JSON', 'Ext.Deferred', 'Ext.Promise'],

    getBookTracks: function (bookId, count) {
        return new Ext.Promise(function (resolve, reject) {
            Ext.Ajax.request({
                url: Constants.APPLICATION_BASE_URL + 'api/booktracks/' + count + '/' + bookId,
                method: 'GET',
                success: function (response) {
                    var bookTracks = Ext.decode(response.responseText);
                    resolve(bookTracks);
                },
                failure: function (response) {
                    reject(response.status);
                }
            });
        });
    },

    createBookTrack: function (bookTrack) {
        return new Ext.Promise(function (resolve, reject) {
            Ext.Ajax.request({
                url: Constants.APPLICATION_BASE_URL + 'api/booktracks',
                method: 'POST',
                jsonData: bookTrack,
                success: function (response) {
                    resolve();
                },
                failure: function (response) {
                    reject(response.responseText);
                }
            });
        });
    },

    isBookAvailableToUser: function (userLogin, bookId) {
        return new Ext.Promise(function (resolve, reject) {
            Ext.Ajax.request({
                url: Constants.APPLICATION_BASE_URL + 'api/booktracks/user/' + userLogin + '/' + bookId,
                method: 'GET',
                success: function (response) {
                    var isBookAvailableToUser = Ext.decode(response.responseText);
                    resolve(isBookAvailableToUser);
                },
                failure: function (response) {
                    reject(response.responseText);
                }
            });
        });
    }
});