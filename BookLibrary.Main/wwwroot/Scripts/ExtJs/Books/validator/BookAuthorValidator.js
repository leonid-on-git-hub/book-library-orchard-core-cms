Ext.define('BookLibrary.UI.Books.validator.BookAuthorValidator', {
    extend: 'Ext.data.validator.Validator',
    alias: 'validator.bookauthorvalidator',
    alternateClassName: 'BookAuthorValidator',
    singleton: true,
    validate: function (value) {
        if (value.length > 64) {
            return 'The author name max length is 64 characters';
        }
        return true;
    }
});