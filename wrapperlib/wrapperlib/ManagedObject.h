// ManagedObject.h
#pragma once
//using namespace System;
namespace CLI
{
	template<class T>
	public ref class ManagedObject
	{
	protected:
		T* m_Instance;
	public:
		ManagedObject(T* instance)
			: m_Instance(instance)
		{
		}
		virtual ~ManagedObject()
		{
			if (m_Instance != nullptr)
			{
				delete m_Instance;
				m_Instance = nullptr;
			}
		}
		!ManagedObject()
		{
			if (m_Instance != nullptr)
			{
				delete m_Instance;
				m_Instance = nullptr;
			}
		}
		T* GetInstance()
		{
			return m_Instance;
		}
	};

	using namespace System::Runtime::InteropServices;
	static const char* string_to_char_array(System::String^ string)
	{
		const char* str = (const char*)(Marshal::StringToHGlobalAnsi(string)).ToPointer();
		return str;
	}
}
