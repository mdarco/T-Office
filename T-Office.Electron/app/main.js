const { app, BrowserWindow } = require("electron");

// Module to create native browser window
var mainWindow = null;

// Quit when all windows are closed.
app.on('window-all-closed', function () {
    if (process.platform !== 'darwin') {
        app.quit();
    }
});

app.on('browser-window-created',function(e, window) {
    window.setMenu(null);
});

// This method will be called when Electron has finished
// initialization and is ready to create browser windows
app.on('ready', function () {
    mainWindow = new BrowserWindow({ 
        width: 1366,
        height: 768,
        icon: __dirname + '/images/main-icon.png',
        webPreferences: {
            // for jQuery and other similar libraries to work from script tags
            nodeIntegration: false
        }
    });

    // and load the index.html of the app
    mainWindow.loadURL('file://' + __dirname + '/index.html');

    // Open the devtools.
    mainWindow.openDevTools();

    // Emitted when the window is closed.
    mainWindow.on('closed', function () {
        // Dereference the window object, usually you would store windows
        // in an array if your app supports multi windows, this is the time
        // when you should delete the corresponding element.
        mainWindow = null;
    });
});
