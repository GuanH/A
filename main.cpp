#include<Windows.h>
#include<tchar.h>
#include<d3d11.h>
#include<wrl/client.h>
#include<d3dcompiler.h>
#include<iterator>
#include<time.h>
#include<DirectXMath.h>
#include<iostream>
#pragma comment(lib,"d3d11.lib")
#pragma comment(lib,"D3DCompiler.lib")
#define wnd_width 800u
#define wnd_height 600u
#define wnd_x 0u
#define wnd_y 0u
#define wnd_name "windows"
#define frequency 60
#define cp Microsoft::WRL::ComPtr
#define HR_THROW(H,s) {HRESULT hr = H;if(hr!=S_OK){std::cout<<std::hex<<hr<<std::dec<<std::endl;MessageBox(nullptr,s,"ERROR",MB_OK);}} 
struct Vertex 
{ 
	struct
	{
		float x;
		float y;
	}pos;
	struct 
	{
		unsigned char r;
		unsigned char g;
		unsigned char b;
		unsigned char a;
	}color;
};
clock_t start_time;
cp<ID3D11Device> pDevice;
cp<IDXGISwapChain> pSwap;
cp<ID3D11DeviceContext> pContext;
cp<ID3D11RenderTargetView> pTarget;
cp<ID3D11Resource> pBackBuffer = nullptr;
cp<ID3D11Buffer> pVertexBuffer;
cp<ID3D11Buffer> pIndexBuffer;
cp<ID3D11PixelShader> pPixelShader;
cp<ID3DBlob> pBlob;
cp<ID3D11InputLayout> pInputLayout;
cp<ID3D11VertexShader> pVertexShader;
LRESULT CALLBACK wndProc(HWND hwnd,UINT msg,WPARAM wParam,LPARAM lParam)
{
	switch (msg)
	{
	case WM_DESTROY: {PostQuitMessage(0); break; }
	default: {return DefWindowProc(hwnd, msg, wParam, lParam); break; }
	}
}
int CALLBACK WinMain(HINSTANCE hInstance,HINSTANCE hPrevInstance,LPSTR lpCmdLine,int nCmdShow)
{
	start_time = clock();
	//windows class
	
	WNDCLASSEX wnd;
	ZeroMemory(&wnd, sizeof(wnd));
	wnd.cbClsExtra = 0;
	wnd.cbSize = sizeof(WNDCLASSEX);
	wnd.cbWndExtra = 0;
	wnd.hbrBackground = (HBRUSH)(COLOR_WINDOW);
	wnd.hCursor = LoadCursor(nullptr, IDC_ARROW);
	wnd.hIcon = LoadIcon(nullptr, IDI_APPLICATION);
	wnd.hIconSm = LoadIcon(nullptr, IDI_APPLICATION);
	wnd.hInstance = hInstance;
	wnd.lpfnWndProc = wndProc;
	wnd.lpszClassName = "windows";
	wnd.lpszMenuName = nullptr;
	wnd.style = CS_OWNDC;
	if (!RegisterClassEx(&wnd))
	{
		MessageBox(nullptr, "failed to register windows", "ERROR", MB_OK);
		return 1;
	}
	HWND hwnd = CreateWindowEx(0, "windows", wnd_name, WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX | WS_MAXIMIZEBOX, wnd_x, wnd_y, wnd_width,wnd_height, nullptr, nullptr, hInstance, nullptr);
	if (!hwnd)
	{
		MessageBox(nullptr, "failed to create windows", "ERROR", MB_OK);
		return 1;
	}
	ShowWindow(hwnd, SW_SHOW);
	
		
	//////device
		DXGI_SWAP_CHAIN_DESC sd;
		ZeroMemory(&sd, sizeof(DXGI_SWAP_CHAIN_DESC));
		sd.BufferCount = 1;
		sd.BufferDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
		sd.BufferDesc.Height =wnd_height ;
		sd.BufferDesc.RefreshRate.Denominator =0 ;
		sd.BufferDesc.RefreshRate.Numerator =0 ;
		sd.BufferDesc.Scaling =DXGI_MODE_SCALING_UNSPECIFIED ;
		sd.BufferDesc.ScanlineOrdering =DXGI_MODE_SCANLINE_ORDER_UNSPECIFIED ;
		sd.BufferDesc.Width =wnd_width ;		
		sd.BufferUsage =DXGI_USAGE_RENDER_TARGET_OUTPUT ;
		sd.Flags = 0;
		sd.OutputWindow = hwnd;
		sd.SampleDesc.Count =1 ;
		sd.SampleDesc.Quality =0 ;
		sd.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;
		sd.Windowed = TRUE;
		HR_THROW(D3D11CreateDeviceAndSwapChain(nullptr, D3D_DRIVER_TYPE_HARDWARE, nullptr, 0, nullptr, 0, D3D11_SDK_VERSION, &sd, pSwap.ReleaseAndGetAddressOf(), pDevice.ReleaseAndGetAddressOf(), nullptr, pContext.ReleaseAndGetAddressOf()),"80 :D3D11CreateDeviceAndSwapChain failed");
		
		HR_THROW(pSwap->GetBuffer(0, __uuidof(ID3D11Resource), &pBackBuffer),"82: pSwap->GetBuffer failed");
		HR_THROW(pDevice->CreateRenderTargetView(pBackBuffer.Get(), nullptr, pTarget.ReleaseAndGetAddressOf()),"83: pDevice->CreateRenderTargetView failed");
	
	
	

	//windows msg process
	
		MSG msg;
		msg.message = WM_NULL;
		while (msg.message != WM_QUIT)
		{			
			double duration_time = (double)(clock() - start_time)/CLOCKS_PER_SEC;
			if (PeekMessage(&msg, nullptr, 0u, 0u, PM_REMOVE))
			{
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			};
			
			
			//render
			//clean
			float color[4] = {sin(duration_time),sin(duration_time+1.0f),1.0f,1.0f};
			pContext->ClearRenderTargetView(pTarget.Get(), color);
			
				
				//triangle
				const Vertex vertices[] = 
				{ 
					{ 0.0f, 1.0f, 225, 0, 0,0},
					{ 1.0f,-1.0f, 0, 225, 0,0},
					{-1.0f,-1.0f, 0, 0 ,225,0}
				};
				
				D3D11_BUFFER_DESC bd;
				ZeroMemory(&bd, sizeof(bd));
				bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;
				bd.ByteWidth = sizeof(vertices);
				bd.CPUAccessFlags = 0u;
				bd.MiscFlags = 0u;
				bd.StructureByteStride = sizeof(Vertex);
				bd.Usage = D3D11_USAGE_DEFAULT;

				D3D11_SUBRESOURCE_DATA sbd;
				ZeroMemory(&sbd, sizeof(D3D11_SUBRESOURCE_DATA));
				sbd.pSysMem = vertices;
				HR_THROW(pDevice->CreateBuffer(&bd, &sbd, &pVertexBuffer), "101: pDevice->CreateBuffer failed");
				
				//bind vertex buffer to the pipeline
				const UINT stride = sizeof(Vertex);
				const UINT offset = 0u;
				pContext->IASetVertexBuffers(0u, 1u, pVertexBuffer.GetAddressOf(), &stride, &offset);

				//create index buffer

				const unsigned short indices[] = 
				{
					0,1,2 
				};
				
				D3D11_BUFFER_DESC ibd;
				ZeroMemory(&ibd, sizeof(ibd));
				ibd.BindFlags = D3D11_BIND_INDEX_BUFFER;
				ibd.ByteWidth = sizeof(indices);
				ibd.CPUAccessFlags = 0u;
				ibd.MiscFlags = 0u;
				ibd.StructureByteStride = sizeof(unsigned short);
				ibd.Usage = D3D11_USAGE_DEFAULT;

				D3D11_SUBRESOURCE_DATA isd;
				ZeroMemory(&isd, sizeof(isd));
				isd.pSysMem = indices;
				HR_THROW(pDevice->CreateBuffer(&ibd, &isd, &pIndexBuffer), "101: pDevice->CreateBuffer failed");
				pContext->IASetIndexBuffer(pIndexBuffer.Get(),DXGI_FORMAT_R16_UINT,0u);

				
				//create PixelShader
				
				D3DReadFileToBlob(L"PixelShader.cso", &pBlob);
				HR_THROW(pDevice->CreatePixelShader(pBlob->GetBufferPointer(), pBlob->GetBufferSize(), nullptr, &pPixelShader), "111: pDevice->CreatePixelShader failed");
				//bind PixelShader
				pContext->PSSetShader(pPixelShader.Get(), nullptr, 0u);
				//create VertexShader
				

				HR_THROW(D3DReadFileToBlob(L"VertexShader.cso", &pBlob), "117: D3DReadFileToBlob");
				HR_THROW(pDevice->CreateVertexShader(pBlob->GetBufferPointer(), pBlob->GetBufferSize(), nullptr, &pVertexShader), "118: pDevice->CreateVertexShader failed");
				//bind VertexShader
				pContext->VSSetShader(pVertexShader.Get(), nullptr, 0u);

				//input and output
				
				const D3D11_INPUT_ELEMENT_DESC ied[] =
				{
					{"Position",0,DXGI_FORMAT_R32G32_FLOAT,0,0,D3D11_INPUT_PER_VERTEX_DATA,0},
					{"Color",0,DXGI_FORMAT_R8G8B8A8_UNORM,0,D3D11_APPEND_ALIGNED_ELEMENT,D3D11_INPUT_PER_VERTEX_DATA,0}
				};
				HR_THROW(pDevice->CreateInputLayout(ied, UINT(std::size(ied)), pBlob->GetBufferPointer(), pBlob->GetBufferSize(), &pInputLayout), "128: pDevice->CreateInputLayout failed");
				//bind InputLayout
				pContext->IASetInputLayout(pInputLayout.Get());
				

				//set primitive topology
				pContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY::D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

				//viewport
				D3D11_VIEWPORT vp;
				vp.Height = wnd_height;
				vp.MaxDepth = 1.0f;
				vp.MinDepth = 0.0f;
				vp.TopLeftX = 0;
				vp.Width = wnd_width;
				pContext->RSSetViewports(1u, &vp);


				//draw							
				pContext->DrawIndexed(UINT(std::size(indices)),0u, 0u);
				//bind render target
				pContext->OMSetRenderTargets(1u, pTarget.GetAddressOf(), nullptr);
				HR_THROW(pSwap->Present(1u, 0u), "151: pSwap->Present failed");


			
				Sleep(1000/frequency);
		}
	
}




