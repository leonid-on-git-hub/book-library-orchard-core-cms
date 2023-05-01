Ext.define('BookLibrary.UI.Books.controller.BookController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.bookcontroller',

    onClose: function () {
        this.getView().close();
    },

    onSave: function () {
        var me = this;
        var book = me.createBookModel(me);
        if (this.isFormValid() && book.isValid()) {
            BookService.createBook(book.data).then(function (createdBook) {
                UserService.getUserName().then(function(userName){
                    var createBookTrack = {
                        BookId: createdBook.id,
                        UserName: userName,
                        UserEmail: '',
                        ActionDate: new Date(),
                        Action: 'Created'
                    }
                    BookTracksService.createBookTrack(createBookTrack);
                    me.getView().close();
                }, function (errorText) {
                    Ext.Msg.alert('Error', errorText);
                });
            }, function (errorText) {
                Ext.Msg.alert('Error', errorText);
            });
        }
    },

    onEdit: function () {
        var me = this;
        var book = me.createBookModel(me);
        if (this.isFormValid() && book.isValid()) {
            BookService.updateBook(book.data).then(function () {
                UserService.getUserName().then(function(userName){
                    var availableCheckbox = me.lookupReference('available-checkbox');
                    if(availableCheckbox.isDirty()){
                        var bookTrack = {
                            BookId: book.data.id,
                            UserName: userName,
                            UserEmail: '',
                            ActionDate: new Date(),
                            Action: book.data.available ? 'Put' : 'Taken'
                        }
                        BookTracksService.createBookTrack(bookTrack);
                    }
                    me.getView().close();
                }, function (errorText) {
                    Ext.Msg.alert('Error', errorText);
                });
            }, function (errorText) {
                Ext.Msg.alert('Error', errorText);
            });
            
        }
    },

    createBookModel: function (viewController) {
        var bookData = viewController.getViewModel().get('book');
        bookData.authors = [];
        viewController.getViewModel().getStore('bookAuthorsStore').getRange().forEach(author => {
            var authorName = author.getData().name;
            if (!!authorName) {
                bookData.authors.push(authorName);
            }
        });
        return Ext.create('model.book', bookData);
    },

    isFormValid: function () {
        return this.getView().down('form').getForm().isValid();
    }
});