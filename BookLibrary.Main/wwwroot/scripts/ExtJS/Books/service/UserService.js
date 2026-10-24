Ext.define('BookLibrary.UI.Books.service.UserService', {
    alternateClassName: 'UserService',
    singleton: true,
    requires: ['Ext.Ajax', 'Ext.JSON', 'Ext.Deferred', 'Ext.Promise'],

    getUserName: function () {
        return new Ext.Promise(function (resolve, reject) {
            Ext.Ajax.request({
                url: Constants.APPLICATION_BASE_URL + 'api/user/name',
                method: 'GET',
                success: function (response) {
                    var userName = response.responseText;
                    resolve(userName);
                },
                failure: function (response) {
                    reject(response.status);
                }
            });
        });
    }
});