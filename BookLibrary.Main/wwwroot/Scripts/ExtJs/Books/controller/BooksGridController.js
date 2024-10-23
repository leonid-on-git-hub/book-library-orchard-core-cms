Ext.define('BookLibrary.UI.Books.controller.BooksGridController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.booksgridcontroller',

    onAdd: function () {
        var me = this;
        var mask = me.createMask(me);
        mask.show();
        var window = Ext.create('bookwindow', {
            title: 'Create book',
            iconCls: 'fas fa-file-signature',
            listeners: {
                close: function () {
                    me.getView().getStore().load();
                }
            }
        });
        mask.destroy();
        window.show();
    },

    onView: function (eventGrid, rowIndex, colIndex) {
        this.openBookWindowAndLoadBook('View book', true, false, rowIndex);
    },

    onEdit: function (eventGrid, rowIndex, colIndex) {
        this.openBookWindowAndLoadBook('Edit book', false, false, rowIndex);
    },

    onDelete: function (eventGrid, rowIndex, colIndex) {
        var me = this;
        Ext.Msg.show({
            title: 'Confirm',
            message: 'Delete book?',
            buttons: Ext.Msg.YESNO,
            icon: Ext.Msg.QUESTION,
            fn: function (button) {
                if (button === 'yes') {
                    var grid = me.getView();
                    var mask = me.createMask(me);
                    mask.show();
                    var selectedRecord = isNaN(rowIndex) ? grid.getSelectionModel().getSelection()[0] : grid.getStore().getAt(rowIndex);
                    BookService.deleteBook(selectedRecord.data.id).then(function () {
                        me.getView().getStore().load();
                        mask.destroy();
                    }), function (errorText) {
                        Ext.Msg.alert('Error', errorText);
                        mask.destroy();
                    };
                }
            }
        });
    },

    onClearSearch: function () {
        var viewModel = this.getViewModel();
        viewModel.set('searchBookName', '');
        viewModel.set('searchAuthor', '');
        viewModel.set('searchYear', null);
        viewModel.set('searchOnlyAvailable', false);
        this.getView().lookupReference('searchYear').setValue('');
        this.getView().getStore().getProxy().setExtraParams({});
        this.onSearch();
    },

    onSearch: function () {
        this.getView().getStore().loadPage(1);
    },

    onGridContextMenu: function (view, rec, node, index, e) {
        e.stopEvent();
        this.getContextMenu().show().setLocalXY(e.getXY());
        return false;
    },

    getContextMenu: function () {
        return this.contextMenu || (this.contextMenu = this.view.add({
            xtype: 'menu',
            items: [
                '@view',
                '@edit',
                '@delete'
            ]
        }));
    },

    openBookWindowAndLoadBook: function (windowTitle, readOnly, isNew, rowIndex) {
        var me = this;
        var grid = me.getView();
        var selectedRecord = isNaN(rowIndex) ? grid.getSelectionModel().getSelection()[0] : grid.getStore().getAt(rowIndex);
        if (selectedRecord) {
            var mask = me.createMask(me);
            mask.show();
            var iconCls = readOnly ? 'fa fa-file-alt' : 'fas fa-file-signature';
            BookService.getBook(selectedRecord.id).then(function (bookData) {
                bookData.year = new Date(bookData.year);
                var authors = [];
                bookData.authors.forEach(author => {
                    authors.push({ name: author });
                });
                UserService.getUserName().then(function (userName) {
                    BookTracksService.isBookAvailableToUser(userName, bookData.id).then(function (isBookAvailableToUser) {
                        BookTracksService.getBookTracks(bookData.id, 10).then(function (bookTracks) {
                            var window = Ext.create('bookwindow', {
                                iconCls: iconCls,
                                title: windowTitle,
                                viewModel: Ext.create('viewmodel.bookviewmodel', {
                                    stores: {
                                        bookAuthorsStore: {
                                            fields: ['name'],
                                            data: authors
                                        }
                                    },
                                    data: {
                                        book: bookData,
                                        bookTracks: bookTracks,
                                        readOnly: readOnly,
                                        bookAvailableReadOnly: !isBookAvailableToUser,
                                        isNew: isNew,
                                        authorsSelection: null
                                    }
                                }),
                                listeners: {
                                    close: function () {
                                        if (!readOnly) {
                                            me.getView().getStore().load();
                                        }
                                    }
                                }
                            });
                            mask.destroy();
                            window.show();
                        }, function (errorText) {
                            Ext.Msg.alert('Error', errorText);
                        });
                    }, function (errorText) {
                        Ext.Msg.alert('Error', errorText);
                    })
                }, function (errorText) {
                    Ext.Msg.alert('Error', errorText);
                })
            }, function (error) {
                grid.getStore().load();
                mask.destroy();
            });
        }
    },

    createMask: function (controller) {
        return new Ext.LoadMask({
            msg: 'Please, wait...',
            target: controller.getView()
        });
    }
});