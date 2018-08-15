#include <iostream>
using namespace std;

template <class T>  //ȥ���������ݼ��������е�ֵ��ͬ�Ľڵ� 
struct Node
{
	T data;
	Node <T> * next; 
};


template <class T>
class LinkList
{
	public:
		LinkList(T a[],int n);
		void ShowList();
		void DeleteSam();
	    
	private:
		Node<T> * first; 
 };
 
template <class T>  		//ͷ�巨���������� 
 LinkList<T> :: LinkList(T a[], int n)  	//���в����Ĺ��캯�� 
 {
 	  
	 first = new Node <T>;
	 first -> next =NULL;
 	 for(int i=0;i<n;i++)
 	 {
 		Node<T> * s;
 		s = new Node <T>;
 		s->data = a[i];
 		s->next = first->next;
 		first->next = s;
 			
	 }
	 
 	}
 	
 template <class T>  
 void  LinkList<T> :: ShowList()   //������� 
 {  
    Node<T> * s;
    s=first->next;
 	while(s!=NULL){ 
 	
 	cout<< s->data <<" ";
 	s=s->next;
	 	
 }
 	
 }
 
 template <class T>  
 void  LinkList<T> :: DeleteSam()
 {  
    Node<T> * p;
 	p=first;
	while(p->next!=NULL)
			if(p->data==p->next->data){
				Node<T> * q; 
				q=p->next;
				p->next=q->next;   //��ֵ��ͬ�ڵ��еĺ�һ���ڵ�ɾ�� 
				delete q;
				
			} 
			else p=p->next;
 	
 }
 
 int main(){
 	
     int a[]={1,2,3,5,5};
 	 LinkList <int > list(a,5);
 	 
 	 
 	 
 	cout<<"ɾ���ظ�Ԫ��֮ǰ���������е���������Ϊ��"<<endl; 
 	list.ShowList();
 	cout<<endl;
 	
 	cout<<"ɾ���ظ�Ԫ��֮�󣬵������е���������Ϊ��"<<endl; 
 	list.DeleteSam();
 	list.ShowList();
 	cout<<endl;
 	
 	return 0;
 }
