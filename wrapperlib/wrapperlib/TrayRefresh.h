// wrapperlib\TrayRefresh.h
#pragma once
#include "ManagedObject.h"
#include "../../nativelib/nativelib/nativelib.h"
//using namespace System;
namespace CLI
{
	public ref class TrayRefresh : public ManagedObject<nativelib::TrayRefresh>
	{
	public:
		TrayRefresh();
	};
}
