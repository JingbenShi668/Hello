# include <iostream>
using namespace std;

const int Maxsize=10;
template <class T>      //ʹ����ģ�� 
class SeqList
{
	public:
	   SeqList(T a[],int n);
	   void reverse (T a[],int n);
	   void print(T a[],int n);
	 
	 private: 
	 T data[Maxsize];   
	
};


template <class T>
SeqList<T> :: SeqList(T a[],int n)    //�������캯�� 
{
	for(int i=0; i<n; i++)
	
		data[i]=a[i];
			
}

template <class T>
void SeqList<T> :: reverse(T a[],int n)    //�������ú��� 
{
		for(int j=0; j<n/2; j++){   // j < n/2,��� j<n,��������η�ת����ᱣ��ԭ���� 
				T temp;
				temp=a[j];
				a[j]=a[n-j-1];
				a[n-j-1]=temp;
            }
          
}
		
template <class T>
void SeqList<T> :: print(T a[],int n)
{
	for(int i=0; i<n; i++)  
	cout<<" "<<a[i];
				
}		
		
			
int main(){
		
			int arr[6]={1,2,7,8,9,10};
			
		    SeqList <int> list(arr,6);  //����SeqList�Ķ��� 
		    
		    cout<<"��ʼ����Ϊ��"<<endl;
		    list.print(arr,6);
		    cout<<endl;
			
			cout<<"ת������Ϊ��"<<endl;
			list.reverse(arr,6); 
		    list.print(arr,6);
		    cout<<endl;
		    
		    return 0;
				
		}
		
		
			
			
			
		



  
    	
    	
	 
	
 


