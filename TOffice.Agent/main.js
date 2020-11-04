const { app, BrowserWindow, Tray, Menu, ipcMain } = require('electron');
const url = require('url');
const path = require('path');
const macaddress = require('macaddress');

// Keep a global reference of the window object, if you don't, the window will
// be closed automatically when the JavaScript object is garbage collected.
let win = null;
let tray = null;

function createWindow() {
    // Create the browser window
    win = new BrowserWindow({
        width: 800,
        height: 600,
        icon: path.join(__dirname, '/assets/beetle.png'),
        webPreferences: {
            nodeIntegration: true
        },
        center: true,
        autoHideMenuBar: true,
        resizable: false,
        // transparent: true
    });

    // and load the index.html of the app
    // win.loadFile('index.html');
    let promiseLoadURL = win.loadURL(
        url.format({
            pathname: path.join(__dirname, '/app/dist/index.html'),
            protocol: 'file:',
            slashes: true
        })
    );

    // Open the DevTools.
    win.webContents.openDevTools();

    win.on('minimize', (event) => {
        event.preventDefault();
        win.hide();
    });

    win.on('restore', () => {
        win.show();
    });

    // Emitted when the window is closed.
    win.on('closed', () => {
        // Dereference the window object, usually you would store windows
        // in an array if your app supports multi windows, this is the time
        // when you should delete the corresponding element.
        win = null;
    });
}

function createTray() {
    tray = new Tray(path.join(__dirname, '/assets/beetle.png'));

    if (process.platform === 'win32') {
        tray.on('click', tray.popUpContextMenu);
    }

    const menu = Menu.buildFromTemplate([
        {
            label: 'Open',
            click() { win.show(); }
        },
        {
            label: 'Quit',
            click() { app.quit(); }
        }
    ]);

    tray.setToolTip('T-Office');
    tray.setContextMenu(menu);

    tray.on('double-click', () => {
        win.show();
    });
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.on('ready', () => {
    createWindow();
    createTray();

    // hide dock icon
    // if (app.dock) app.dock.hide();
});

// Quit when all windows are closed.
app.on('window-all-closed', () => {
    // On macOS it is common for applications and their menu bar
    // to stay active until the user quits explicitly with Cmd + Q
    if (process.platform !== 'darwin') {
        app.isQuitting = true;
        app.quit();
    }
});

app.on('activate', () => {
    // On macOS it's common to re-create a window in the app when the
    // dock icon is clicked and there are no other windows open.
    if (win === null) {
        createWindow();
    }
});

ipcMain.on('getAgentId', (event, arg) => {
    macaddress.one((err, mac) => {
        console.log('MAC Address: ' + mac);
        win.webContents.send('setAgentId', mac.replaceAll(':', ''));
    });
});
