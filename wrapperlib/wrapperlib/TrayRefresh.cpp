// wrapperlib\TrayRefresh.cpp
#include "TrayRefresh.h"
namespace CLI
{
	TrayRefresh::TrayRefresh()
		: ManagedObject(new nativelib::TrayRefresh())
	{
		// actual code in the new above
	}
	//void TrayRefresh::Move(float deltaX, float deltaY)
	//{
	//	m_Instance->Move(deltaX, deltaY);
	//}
}
