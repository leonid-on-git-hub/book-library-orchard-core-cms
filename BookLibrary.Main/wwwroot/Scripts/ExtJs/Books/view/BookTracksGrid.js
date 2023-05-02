Ext.define('BookLibrary.UI.Books.view.BookTracksGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.booktracksgrid',
    title: 'Tracks',
    collapsible: false,
    viewConfig: {
        enableTextSelection: true
    },
    columns: [{
        dataIndex: 'userName',
        text: 'User',
        flex: 1
    }, {
        dataIndex: 'userEmail',
        text: 'Email',
        flex: 1
    }, {
        dataIndex: 'actionDate',
        text: 'Date',
        flex: 1.1,
        renderer: function (value) {
            return Ext.Date.format(new Date(value), Constants.DATETIME_FORAMAT);
        }
    }, {
        dataIndex: 'action',
        text: 'Action',
        flex: 1
    }],
});