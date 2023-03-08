// TrayCleanup.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <windows.h>
#include <Commctrl.h>

int main()
{
	HWND finder = FindWindow(L"Shell_TrayWnd", NULL);

	if (finder) {
		finder = FindWindowEx(finder, NULL, L"TrayNotifyWnd", NULL);

		if (finder) {
			finder = FindWindowEx(finder, NULL, L"SysPager", NULL);

			if (finder) {
				finder = FindWindowEx(finder, NULL, L"ToolbarWindow32", NULL);

				int IconsCount = (int)SendMessage(finder, TB_BUTTONCOUNT, 0, 0);

				std::cout << "icons count: " << IconsCount << "\n";

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

								std::cout << "icon: " << IconsCount << "\n";

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
										/*SendMessage(finder, TB_DELETEBUTTON, IconsCount, 0);
										SendMessage(finder, WM_WININICHANGE, 0, 0);*/

										std::cout << "deleting dead\n";
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

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
