Ext.define('BookLibrary.UI.Books.controller.BookAuthorsGridController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.bookauthorsgridcontroller',

    onAdd: function () {
        var view = this.getView();
        var firsRecord = view.getStore().getAt(0);
        if (!firsRecord || !!firsRecord.data.name) {
            view.store.insert(0, { name: '' });
        }
        view.getSelectionModel().select(0, true);
        view.findPlugin('cellediting').startEditByPosition({
            row: 0,
            column: 0
        });
    },

    onDelete: function () {
        var me = this;
        var grid = me.getView();
        var selectedRecord = me.getViewModel().get('authorsSelection');
        if(!selectedRecord.data.name || selectedRecord.data.name.match(/^ *$/) !== null){
            grid.store.remove(selectedRecord);
        } else {
            Ext.Msg.show({
                title: 'Confirm',
                message: 'Delete author?',
                buttons: Ext.Msg.YESNO,
                icon: Ext.Msg.QUESTION,
                fn: function (button) {
                    if (button === 'yes') {
                        grid.store.remove(selectedRecord);
                    }
                }
            });
        }
    },

    onGridContextMenu: function (view, rec, node, index, e) {
        e.stopEvent();
        this.getContextMenu().show().setLocalXY(e.getXY());
        return false;
    },

    privates: {
        getContextMenu: function () {
            return this.contextMenu || (this.contextMenu = this.view.add({
                xtype: 'menu',
                items: [
                    '@delete'
                ]
            }));
        }
    }
});