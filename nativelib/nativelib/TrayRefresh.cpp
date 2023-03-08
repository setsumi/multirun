// nativelib\TrayRefresh.cpp
#include "TrayRefresh.h"
#include <windows.h>
#include <Commctrl.h>
#pragma comment(lib, "user32")
#pragma comment(lib, "Shell32")
namespace nativelib
{
	// Remove residue tray icons
	TrayRefresh::TrayRefresh()
	{
		HWND finder = FindWindow(L"Shell_TrayWnd", NULL);

		if (finder) {
			finder = FindWindowEx(finder, NULL, L"TrayNotifyWnd", NULL);

			if (finder) {
				finder = FindWindowEx(finder, NULL, L"SysPager", NULL);

				if (finder) {
					finder = FindWindowEx(finder, NULL, L"ToolbarWindow32", NULL);

					int IconsCount = (int)SendMessage(finder, TB_BUTTONCOUNT, 0, 0);

					//std::cout << "icons count: " << IconsCount << "\n";

					if (IconsCount) {
						DWORD ProcId;
						GetWindowThreadProcessId(finder, &ProcId);

						HANDLE hProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ, FALSE, ProcId);

						if (hProcess) {
							LPVOID lpData = VirtualAllocEx(hProcess, NULL, sizeof(TBBUTTON), MEM_COMMIT, PAGE_READWRITE);

							if (lpData) {
								for (;;) {
									if (IconsCount <= 0) {
										break;
									}
									IconsCount--;

									//std::cout << "icon: " << IconsCount << "\n";

									TBBUTTON button;
									SIZE_T BytesRead;

									SendMessage(finder, TB_GETBUTTON, IconsCount, (LPARAM)lpData);
									if (ReadProcessMemory(hProcess, lpData, &button, sizeof(TBBUTTON), &BytesRead)) {
										if (BytesRead != sizeof(TBBUTTON)) {
											break;
										}
										struct EXTRADATA {
											HWND Wnd; // icon parent window handle
											UINT uID; // icon style
										} extra{};
										if (ReadProcessMemory(hProcess, (LPVOID)button.dwData, &extra, sizeof(EXTRADATA), &BytesRead)) {
											if (BytesRead != sizeof(EXTRADATA)) {
												break;
											}

											if (extra.uID & 0x80000000) {
												continue;
											}

											if (IsWindow(extra.Wnd)) {
												continue;
											}

											NOTIFYICONDATA nid;
											nid.cbSize = sizeof(NOTIFYICONDATA);
											nid.hWnd = extra.Wnd;
											nid.uID = extra.uID;

											// way 1
											Shell_NotifyIcon(NIM_DELETE, &nid);

											// way 2
											//SendMessage(finder, TB_DELETEBUTTON, IconsCount, 0);
											//SendMessage(finder, WM_WININICHANGE, 0, 0);

											//std::cout << "deleting dead\n";
										}
									}
									else {
										break;
									}
								}
								VirtualFreeEx(hProcess, lpData, 0, MEM_RELEASE);
								CloseHandle(hProcess);
							}
						}
					}
				}
			}
		}
	}
}
